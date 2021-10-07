using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.Entities;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface IOrganisationInviteRepository: IAsyncRepository<OrganisationInvite>
    {
        Task<PaginationResponse<OrganisationInviteDb>> GetInvitesAsync(PaginationParameters parameters);
        Task<Organisation> RespondOnInviteAsync(int inviteId, bool response);
    }
}