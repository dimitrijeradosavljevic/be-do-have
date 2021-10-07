

using System;
using System.Collections.Generic;

namespace BeDoHave.ElasticSearch.Entities
{
    public class PageSearchParameters
    {
        public string? Term { get; set; }
        public bool? TitleOnly { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public DateTime? UpdatedAtStart { get; set; }
        public DateTime? UpdatedAtEnd { get; set; }
        public int? PageId { get; set; }
        
        public IList<int> TagIds { get; set; }
        public Dictionary<int, List<string>> Tags { get; set; } 
        public int? OrganisationId { get; set; }
        public IEnumerable<object> InPagesSearch { get; set; }
    }
}