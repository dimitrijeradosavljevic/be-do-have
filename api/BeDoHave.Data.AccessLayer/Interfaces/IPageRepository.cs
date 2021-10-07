using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface IPageRepository: IAsyncRepository<Page>
    {
        Task<ICollection<Page>> GetByIdsAsync(IList<int> ids);
        Task<PaginationResponse<Page>> GetPagesForPickerAsync(PaginationParameters parameters, IList<int> organisationsIds);
        Task TrashPageAsync(Page page, bool archived);
        Task DeletePageAsync(int pageId);
        Task UpdatePageAsync(Page page, bool updateTitle);
        Task LoadUserAsync(Page page);
        Task MovePageAsync(int pageId, int directParentId);
        Task<Organisation> MovePageUnderOrganisationAsync(int pageId, int organisationId);
        Task<PaginationResponse<Page>> GetTrashedPagesAsync(PaginationParameters parameters);
    }
}