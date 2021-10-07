using System;
using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Application.Specifications;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;
using BeDoHave.Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BeDoHave.ElasticSearch.Entities;
using BeDoHave.ElasticSearch.Interfaces;

namespace BeDoHave.Application.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IOrganisationService _organisationService;
        private readonly ISearchPageRepository _searchPageRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public PageService(
            IPageRepository pageRepository,
            IOrganisationService organisationService,
            ISearchPageRepository searchPageRepository,
            ITagRepository tagRepository,
            IMapper mapper,
            ITagService tagService)
        {
            _pageRepository = pageRepository;
            _organisationService = organisationService;
            _searchPageRepository = searchPageRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
            _tagService = tagService;
        }

        public async Task<PaginationResponse<Page>> GetPagesForPickerAsync(PaginationParameters paginationParameters, int userId)
        {
            var organisations = await _organisationService.GetAllOrganisationsAsync(userId);
            
            return await _pageRepository.GetPagesForPickerAsync(paginationParameters, organisations.Select(o => o.Id).ToArray());
        }

        public async Task<IList<PageDTO>> GetAllPagesAsync()
        {
            var pages = await _pageRepository.GetAsync();

            return _mapper.Map<IList<PageDTO>>(pages);
        }

        public async Task<IList<PageDTO>> GetPagesAsync(PaginationParameters paginationParameters)
        {
            var pages = await _pageRepository.GetBySpecAsync(
                new PageSpecification(
                    page => page.Title.Contains(paginationParameters.Keyword),
                    start: paginationParameters.PageIndex * paginationParameters.PageSize,
                    take: paginationParameters.PageSize,
                    orderBy: paginationParameters.OrderBy,
                    direction: paginationParameters.Direction));

            return _mapper.Map<IList<PageDTO>>(pages);
        }

        public async Task<PageDTO> GetPageByIdAsync(int pageId)
        {
            var includes = new List<Expression<Func<Page, object>>>();
            includes.Add(p => p.AncestorsLinks.OrderByDescending(al => al.Depth));
            includes.Add(p => p.Tags);
            includes.Add(p => p.Descendants);
            includes.Add(p => p.User);

            var pages = await _pageRepository.GetBySpecAsync(
                new PageSpecification(
                    p => p.Id == pageId,
                    includes));
            var page = pages.First();

            if (page is null)
            {
                throw new ApiException($"Page: {pageId} not found", 404);
            }

            var pageDto = _mapper.Map<PageDTO>(page);
            pageDto.Tags = _mapper.Map<ICollection<TagDto>>(page.Tags.Where(t => !t.Name.Contains("_id:")));
            pageDto.Path = BuildPath(pageDto.AncestorsLinks, pageDto.Title);

            return pageDto;
        }

        public async Task<PageDTO> CreatePageAsync(CreatePageDTO createPageDTO)
        {
            var page = _mapper.Map<Page>(createPageDTO);
            page.Tags = new List<Tag>();
            
            // TODO (check if organisation exists)

            page.TagPages = new List<TagPage>();
            if (createPageDTO.Tags.Count() > 0)
            {
                foreach (var tag in createPageDTO.Tags)
                {
                    page.TagPages.Add(new TagPage()
                    {
                        TagId = tag.Id
                    });
                }
            }

            if (createPageDTO.DirectPageId is not null)
            {
                page.OrganisationDirect = false;
                var includes = new List<Expression<Func<Page, object>>>();
                includes.Add(p => p.AncestorsLinks);
                
                Page directParent = await _pageRepository.GetSingleBySpecAsync(
                    new PageSpecification(p => p.Id.Equals(createPageDTO.DirectPageId), includes));

                if(directParent is null)
                {
                    throw new ApiException($"Direct parent age {createPageDTO.DirectPageId} not found", 404);
                }

                IList<PageLink> ancestorPageLinks = new List<PageLink>();
                ancestorPageLinks.Add(
                        new PageLink
                        {
                            AncestorPage = directParent,
                            Depth = 1
                        });
                
                foreach(PageLink ancestor in directParent.AncestorsLinks)
                {
                    ancestorPageLinks.Add(
                        new PageLink
                        {
                            AncestorPageId = ancestor.AncestorPageId,
                            Depth = ancestor.Depth + 1
                        });
                }
                
                page.AncestorsLinks = ancestorPageLinks;
            }
            else
            {
                page.OrganisationDirect = true;
            }

            await _pageRepository.AddAsync(page);
            await _tagRepository.AddDefaultTagAsync(page.Id);
            
            await _pageRepository.LoadUserAsync(page);

            var pageSearch = _mapper.Map<PageSearch>(page);
            pageSearch.OrganisationId = createPageDTO.OrganisationId;
            pageSearch.Tags = _mapper.Map<IList<Tag>>(createPageDTO.Tags);
            await _searchPageRepository.IndexSingleAsync(pageSearch);

            return _mapper.Map<PageDTO>(page);
        }

        public async Task<PageDTO> UpdatePageAsync(UpdatePageDTO updatePageDTO)
        {
            var includes = new List<Expression<Func<Page, object>>>();
            includes.Add(p => p.AncestorsLinks);
            includes.Add(p => p.DescendantsLinks);
            includes.Add(p => p.TagPages);
            
            var pages = await _pageRepository.GetBySpecAsync(
                new PageSpecification(
                    p => p.Id == updatePageDTO.Id,
                    includes));
            var page = pages.First();
            
            if (page is null)
            {
                throw new ApiException($"Page {updatePageDTO.Id} not found", 404);
            }

            var titleChanged = page.Title != updatePageDTO.Title;

            _mapper.Map(updatePageDTO, page);
            await _pageRepository.UpdatePageAsync(page, titleChanged);

            return _mapper.Map<PageDTO>(page);
        }

        public async Task DeletePageAsync(int pageId)
        {
            // TODO obradi situaciju da page ne postoji

            await _pageRepository.DeletePageAsync(pageId);
        }
        
        public async Task<PaginationResponse<PageDTO>> GetTrashedPagesAsync(PaginationParameters paginationParameters)
        {
            var response = await _pageRepository.GetTrashedPagesAsync(paginationParameters);
            
            // foreach (var pageDto in pageDtos)
            // {
            //     pageDto.Path = BuildPath(pageDto.AncestorsLinks, pageDto.Title);
            // }

            return new PaginationResponse<PageDTO>()
            {
                Items = _mapper.Map<IList<PageDTO>>(response.Items),
                Total = response.Total,
                PageIndex = paginationParameters.PageIndex,
                PageSize = paginationParameters.PageSize
            };
        }
        
        public async Task TrashPageAsync(int pageId, bool archived)
        {
            var includes = new List<Expression<Func<Page, object>>>();
            includes.Add(p => p.Descendants);

            var pages = await _pageRepository.GetBySpecAsync(
                new PageSpecification(
                    p => p.Id == pageId,
                    includes));
            var page = pages.First();

            if (page is null)
            {
                throw new ApiException($"Page {pageId} not found", 404);
            }

            await _pageRepository.TrashPageAsync(page, archived);
        }

        private string BuildPath(ICollection<PageLink> ancestors, string pageTitle)
        {
            var path = "";
            foreach (var ancestorLink in ancestors)
            {
                //path += ancestorLink.AncestorPageTitle + " / ";
            }

            path += pageTitle;
            return path;
        }
        
        public async Task<IList<PageSearchDto>> SearchPagesAsync(PageSearchParameters parameters)
        {
            if (parameters.OrganisationId is not null && parameters.TagIds.Count() > 0)
            {
                var tagWeights = await _tagService.GetTagsHierarchieAsync(parameters.OrganisationId.Value, parameters.TagIds);
                Dictionary<int, List<string>> tags = new Dictionary<int, List<string>>();
                var max = tagWeights[0].Weight;
                foreach (var tagWeight in tagWeights)
                {
                    if (tagWeight.Weight > max)
                    {
                        max = tagWeight.Weight;
                    }
                }
                foreach (var tagWeight in tagWeights)
                {
                    var key = max + 1 - tagWeight.Weight;
                    if (!tags.ContainsKey(key))
                    {
                        tags.Add(key, new List<string>());
                    }
                    List<string> list;
                    tags.TryGetValue(key, out list);
                    list.Add(tagWeight.Id.ToString());
                    tags.TryAdd(key, list);
                }
                parameters.Tags = tags;
            }
            var searchPages = await _searchPageRepository.SearchAsync(parameters);

            List<PageSearchDto> result = new List<PageSearchDto>();
            foreach (var hit in searchPages.Hits)
            {
                var pageSearchDto = _mapper.Map<PageSearchDto>(hit.Source);
                pageSearchDto.HighlightedContent = hit.Highlight;
                result.Add(pageSearchDto);
            }
            
            return result;
        }

        public async Task<string> SearchPageAsync(PageSearchParameters parameters)
        {
            var searchPages = await _searchPageRepository.SearchSingleAsync(parameters);

            if (searchPages.Hits.Count > 0)
            {
                return searchPages.Hits.First().Highlight["content"].First();
            }

            return "";
        }

        public async Task<IList<string>> GetAutoCompleteAsync(string term)
        {
            return await _searchPageRepository.GetAutoCompleteAsync(term);
        }

        public async Task MovePageAsync(int pageId, int directParentId)
        {
            await _pageRepository.MovePageAsync(pageId, directParentId);
        }
        
        public async Task<OrganisationDTO> MovePageUnderOrganisationAsync(int pageId, int organisationId)
        {
            var organisation = await _pageRepository.MovePageUnderOrganisationAsync(pageId, organisationId);

            return _mapper.Map<OrganisationDTO>(organisation);
        }
    }
}
