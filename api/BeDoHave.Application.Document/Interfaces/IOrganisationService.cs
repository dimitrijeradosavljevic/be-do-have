using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Application.Document.DTOs;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Document.Interfaces
{
    public interface IOrganisationService
    {
        Task<IList<OrganisationDTO>> GetOrganisationsAsync(PaginationParameters paginationParameters);
        Task<IList<OrganisationDTO>> GetAllOrganisationsAsync();
        Task<OrganisationDTO> GetOrganisationByIdAsync(int organisationId);
        Task CreateOrganisationAsync(CreateOrganisationDTO createOrganisation);
        Task UpdateOrganisationAsync(UpdateOrganisationDTO updateOrganisation);
        Task DeleteOrganisationAsync(int organisationId);
    }
}
