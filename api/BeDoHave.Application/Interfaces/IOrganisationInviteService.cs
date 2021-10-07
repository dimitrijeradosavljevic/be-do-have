using System.Threading.Tasks;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Interfaces
{
    public interface IOrganisationInviteService
    {
        Task InviteAsync(int organisationId, CreateOrganisationInviteDto crateInviteDto);

        Task<PaginationResponse<OrganisationInviteDto>> GetInvitesAsync(PaginationParameters parameters);
        Task<OrganisationDTO> RespondOnInviteAsync(int inviteId, bool response);
    }
}