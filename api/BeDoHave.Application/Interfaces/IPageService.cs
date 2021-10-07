using BeDoHave.Application.DTOs;
using BeDoHave.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.Core.Entities;
using BeDoHave.ElasticSearch.Entities;

namespace BeDoHave.Application.Interfaces
{
    public interface IPageService
    {
        Task<IList<PageDTO>> GetPagesAsync(PaginationParameters paginationParameters);
        Task<PaginationResponse<Page>> GetPagesForPickerAsync(PaginationParameters paginationParameters, int userId);
        Task<IList<PageDTO>> GetAllPagesAsync();
        Task<PageDTO> GetPageByIdAsync(int pageId);
        Task<PageDTO> CreatePageAsync(CreatePageDTO createPageDTO);
        Task<PageDTO> UpdatePageAsync(UpdatePageDTO updatePageDTO);
        Task DeletePageAsync(int pageId);
        Task TrashPageAsync(int pageId, bool archived);
        Task<PaginationResponse<PageDTO>> GetTrashedPagesAsync(PaginationParameters paginationParameters);
        Task<IList<PageSearchDto>> SearchPagesAsync(PageSearchParameters parameters);
        Task<string> SearchPageAsync(PageSearchParameters parameters);
        Task<IList<string>> GetAutoCompleteAsync(string term);
        Task MovePageAsync(int pageId, int directParentId);
        Task<OrganisationDTO> MovePageUnderOrganisationAsync(int pageId, int organisationId);

    }
}
