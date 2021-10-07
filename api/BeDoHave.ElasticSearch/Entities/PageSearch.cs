using System;
using System.Collections.Generic;
using BeDoHave.Data.Core.Entities;
using Nest;

namespace BeDoHave.ElasticSearch.Entities
{
    public class PageSearch
    {
        [Number(Index = false)]
        public int PageId { get; set; }
        
        [Completion]
        public string Title { get; set; }
        
        [Completion]
        public string Content { get; set; }
        
        public string IconName { get; set; }
        public string IconColor { get; set; }

        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        public Author Author { get; set; }

        public IList<Tag> Tags { get; set; }
        
        public int OrganisationId { get; set; }

        public CompletionField Suggest
        {
            get
            {
                return new CompletionField()
                {
                    Input = new[] {Title, Content}
                };
            }
        }
    }
}