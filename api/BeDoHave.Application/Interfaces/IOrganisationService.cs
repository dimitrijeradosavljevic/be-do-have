using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Application.Dtos;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Interfaces
{
    public interface IOrganisationService
    {
        Task<PaginationResponse<OrganisationDTO>> GetOrganisationsAsync(int userId, PaginationParameters paginationParameters);
        Task<IList<OrganisationDTO>> GetAllOrganisationsAsync(int userId);
        Task<OrganisationDTO> GetOrganisationByIdAsync(int organisationId);
        
        Task<PaginationResponse<UserDto>> GetOrganisationMembersAsync(int organisationId, PaginationParameters paginationParameters);
        Task<PaginationResponse<UserDto>> GetOrganisationNonMembersAsync(int organisationId, PaginationParameters paginationParameters);

        Task AddMemberAsync(int organisationId, int userId);
        
        Task<OrganisationDTO> CreateOrganisationAsync(CreateOrganisationDTO createOrganisation);
        Task<OrganisationDTO> UpdateOrganisationAsync(UpdateOrganisationDTO updateOrganisation);
        Task DeleteOrganisationAsync(int organisationId);


        Task<PageTreeDTO> GetOrganisationTreeAsync(int organisationId);
    }
}
