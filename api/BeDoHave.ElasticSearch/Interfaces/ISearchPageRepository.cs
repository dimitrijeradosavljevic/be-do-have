using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.ElasticSearch.Entities;
using Nest;

namespace BeDoHave.ElasticSearch.Interfaces
{
    public interface ISearchPageRepository
    {
        Task<ISearchResponse<PageSearch>> SearchAsync(PageSearchParameters parameters);
        Task<ISearchResponse<PageSearch>> SearchSingleAsync(PageSearchParameters parameters);
        Task IndexSingleAsync(PageSearch page);

        Task DeleteSingleAsync(int pageId);
        Task<IList<string>> GetAutoCompleteAsync(string term);
    }
}