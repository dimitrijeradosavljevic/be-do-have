using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface IOrganisationRepository: IAsyncRepository<Organisation>
    {
        Task<PaginationResponse<Organisation>> GetUserOrganisationsAsync(int userId, PaginationParameters paginationParameters);
        Task<PaginationResponse<User>> GetMembersAsync(int organisationId, PaginationParameters parameters);
        Task<PaginationResponse<User>> GetNonMembersAsync(int organisationId, PaginationParameters parameters);
        Task AddMemberAsync(int organisationId, int userId);
        Task<IList<Organisation>> GetAllOrganisationsAsync(int userId);
    }
}