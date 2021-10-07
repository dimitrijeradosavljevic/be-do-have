using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeDoHave.ElasticSearch.Entities;
using BeDoHave.ElasticSearch.Interfaces;
using Elasticsearch.Net;
using Nest;

namespace BeDoHave.ElasticSearch.Repositories
{
    public class SearchPageRepository : ISearchPageRepository
    {
        private readonly IElasticClient _elasticClient;

        public SearchPageRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<ISearchResponse<PageSearch>> SearchAsync(PageSearchParameters parameters)
        {
            SearchRequest searchRequest = new SearchRequest();
            BoolQuery boolQuery = new BoolQuery();

            List<QueryContainer> mustMatch = new List<QueryContainer>();
            List<QueryContainer> shouldMatch = new List<QueryContainer>();
            List<QueryContainer> filters = new List<QueryContainer>();

            if (parameters.OrganisationId is not null)
            {
                mustMatch.Add(new MatchQuery()
                {
                    Field = "organisationId",
                    Query = parameters.OrganisationId.ToString()
                });
            }
            
            if (!string.IsNullOrWhiteSpace(parameters.Author))
            {
                mustMatch.Add(new WildcardQuery()
                {
                    Field = "author.fullName",
                    Value = "*" + parameters.Author + "*"
                });
            }
            
            if (parameters.InPagesSearch is not null)
            {
                mustMatch.Add(new TermsQuery()
                {
                    Field = "pageId",
                    Terms = parameters.InPagesSearch,
                });
            }
            
            if (parameters.Tags is not null && parameters.Tags.Any())
            foreach (var tagKeyValue in parameters.Tags)
            {
                shouldMatch.Add(new TermsSetQuery()
                {
                    Field = "tags.id",
                    Terms = tagKeyValue.Value,
                    Boost = tagKeyValue.Key
                });
            }
            
            
            if (!string.IsNullOrWhiteSpace(parameters.Term))
            {
                if (parameters.TitleOnly == true)
                {
                    mustMatch.Add(new WildcardQuery()
                    {
                        Field = "title",
                        Value = "*" + parameters.Term + "*"
                    });
                }
                else
                {
                    shouldMatch.Add(new WildcardQuery()
                    {
                        Field = "title",
                        Value = "*" + parameters.Term + "*"
                    });
                }

                if (parameters.TitleOnly is null || parameters.TitleOnly != true)
                {
                    mustMatch.Add(new WildcardQuery()
                    {
                        Field = "content",
                        Value = "*" + parameters.Term + "*"
                    });
                }
            }


            if (parameters.CreatedAtStart is not null || parameters.CreatedAtEnd is not null)
            {
                var createdAtDataRange = new DateRangeQuery();
                createdAtDataRange.Field = "createdAt";
                if (parameters.CreatedAtStart is not null)
                {
                    createdAtDataRange.GreaterThanOrEqualTo = parameters.CreatedAtStart;
                }

                if (parameters.CreatedAtEnd is not null)
                {
                    createdAtDataRange.LessThanOrEqualTo = parameters.CreatedAtEnd;
                }

                filters.Add(createdAtDataRange);
            }

            if (parameters.UpdatedAtStart is not null || parameters.UpdatedAtEnd is not null)
            {
                var updatedAtDataRange = new DateRangeQuery();
                updatedAtDataRange.Field = "updatedAt";
                if (parameters.UpdatedAtStart is not null)
                {
                    updatedAtDataRange.GreaterThanOrEqualTo = parameters.UpdatedAtStart;
                }

                if (parameters.UpdatedAtEnd is not null)
                {
                    updatedAtDataRange.LessThanOrEqualTo = parameters.UpdatedAtEnd;
                }

                filters.Add(updatedAtDataRange);
            }


            
            boolQuery.Must = mustMatch;
            boolQuery.Should = shouldMatch;
            boolQuery.Filter = filters;

            searchRequest.Query = boolQuery;


            if ((!string.IsNullOrWhiteSpace(parameters.Term) &&
                 (parameters.TitleOnly is null || parameters.TitleOnly != true))
                || !string.IsNullOrWhiteSpace(parameters.Author))
            {
                searchRequest.Highlight = new Highlight()
                {
                    Encoder = HighlighterEncoder.Html,
                    PreTags = new[] {"<b>"},
                    PostTags = new[] {"</b>"},
                    Fields = new Dictionary<Field, IHighlightField>()
                };
                if (!string.IsNullOrWhiteSpace(parameters.Term) &&
                    (parameters.TitleOnly is null || parameters.TitleOnly != true))
                {
                    searchRequest.Highlight.Fields.Add("content", new HighlightField()
                        {
                            Type = HighlighterType.Plain,
                            HighlightQuery = new WildcardQuery() {Value = "*" + parameters.Term + "*"},
                            ForceSource = true,
                            FragmentSize = 100,
                            Fragmenter = HighlighterFragmenter.Span,
                            NumberOfFragments = 3,
                            NoMatchSize = 3
                        }
                    );
                }

                if (!string.IsNullOrWhiteSpace(parameters.Author))
                {
                    searchRequest.Highlight.Fields.Add("author.fullName", new HighlightField()
                    {
                        Type = HighlighterType.Plain,
                        HighlightQuery = new WildcardQuery() {Value = "*" + parameters.Author + "*"},
                        ForceSource = true,
                        FragmentSize = 100,
                        Fragmenter = HighlighterFragmenter.Span,
                        NumberOfFragments = 3,
                        NoMatchSize = 3
                    });
                }
            }

            var pages = await _elasticClient.SearchAsync<PageSearch>(searchRequest);

            return pages;
        }

        public async Task<ISearchResponse<PageSearch>> SearchSingleAsync(PageSearchParameters parameters)
        {
            SearchRequest searchRequest = new SearchRequest();
            BoolQuery boolQuery = new BoolQuery();

            List<QueryContainer> mustMatch = new List<QueryContainer>();
            // List<QueryContainer> shouldMatch = new List<QueryContainer>();

            mustMatch.Add(new TermQuery()
            {
                Field = "pageId",
                Value = parameters.PageId
            });

            // shouldMatch.Add(new WildcardQuery()
            // {
            //     Field = "content",
            //     Value = "*" + parameters.Term + "*"
            // });

            boolQuery.Must = mustMatch;
            // boolQuery.Should = shouldMatch;
            searchRequest.Query = boolQuery;


            searchRequest.Highlight = new Highlight()
            {
                Encoder = HighlighterEncoder.Html,
                PreTags = new[] {"<b>"},
                PostTags = new[] {"</b>"},
                Fields = new Dictionary<Field, IHighlightField>()
            };
            
            searchRequest.Highlight.Fields.Add("content", new HighlightField()
                {
                    Type = HighlighterType.Plain,
                    HighlightQuery = new WildcardQuery()
                    {
                        Field = "content",
                        Value = "*" + parameters.Term + "*"
                    },
                    ForceSource = true,
                    FragmentSize = 100,
                    Fragmenter = HighlighterFragmenter.Span,
                    NumberOfFragments = 3,
                    NoMatchSize = 0
                }
            );

            // searchRequest.StoredFields = new Field("content");

            return await _elasticClient.SearchAsync<PageSearch>(searchRequest);
        }

        public async Task IndexSingleAsync(PageSearch page)
        {
            var response = await _elasticClient.IndexAsync(page, x => x.Index("pages"));
        }

        public async Task DeleteSingleAsync(int pageId)
        {
            var r = await _elasticClient.DeleteByQueryAsync(new DeleteByQueryRequest(Indices.Index("pages"))
            {
                Query = new TermQuery()
                {
                    Field = "pageId",
                    Value = pageId
                }
            });
        }

        public async Task<IList<string>> GetAutoCompleteAsync(string term)
        {
            var response = await _elasticClient.SearchAsync<PageSearch>(search =>
                    search.Index("pages").Suggest(suggest =>
                    suggest.Completion("suggest", completion => completion
                        .Field(page => page.Suggest)
                        .Prefix(term)
                        .Fuzzy(fuzzy => fuzzy.Fuzziness(Fuzziness.EditDistance(1)))
                        .Size(5)))
            );


            var suggest = response.Suggest["suggest"].First();
            List<string> suggestions = new List<string>();
            foreach (var option in suggest.Options)
            {
                suggestions.Add(option.Text);
            }
            return suggestions;
            
            // var searchRequest = new SearchRequest()
            // {
            //     Suggest = new SuggestContainer()
            //     {
            //         {
            //             "s", new SuggestBucket()
            //             {
            //                 Text = term,
            //                 Completion = new CompletionSuggester()
            //                 {
            //                     Field = "suggest"
            //                 }
            //                 
            //             }
            //         }
            //     }
            // };
            // //
            // var suggest1 = await _elasticClient.SearchAsync<PageSearch>(searchRequest);

            return new List<string>();
        }
    }
}