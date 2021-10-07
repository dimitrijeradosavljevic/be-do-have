using System.Collections.Generic;

namespace BeDoHave.Shared.Entities
{
    public class PaginationResponse<T>
    {
        public ICollection<T> Items { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}