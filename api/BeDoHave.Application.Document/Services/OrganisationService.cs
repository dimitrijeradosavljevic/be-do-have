using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Data.AccessLayer.Interfaces;
using AutoMapper;
using BeDoHave.Application.Document.Interfaces;
using BeDoHave.Application.Document.DTOs;
using BeDoHave.Application.Document.Specifications;
using BeDoHave.Shared.Entities;
using BeDoHave.Shared.Exceptions;

namespace BeDoHave.Application.Document.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IAsyncRepository<Organisation> _organisationRepository;
        private readonly IMapper _mapper;

        public OrganisationService(
            IAsyncRepository<Organisation> organisationRepository,
            IMapper mapper)
        {
            _organisationRepository = organisationRepository;
            _mapper = mapper;
        }

        public async Task<IList<OrganisationDTO>> GetAllOrganisationsAsync()
        {
            var organisations = await _organisationRepository.GetAsync();

            return _mapper.Map<IList<OrganisationDTO>>(organisations);
        }

        public async Task<IList<OrganisationDTO>> GetOrganisationsAsync(PaginationParameters paginationParameters)
        {
                var organisations = await _organisationRepository.GetBySpecAsync(
                    new OrganisationSpecification(
                        organisation => organisation.Name.Contains(paginationParameters.Keyword),
                        start: paginationParameters.PageIndex * paginationParameters.PageSize,
                        take: paginationParameters.PageSize,
                        orderBy: paginationParameters.OrderBy,
                        direction: paginationParameters.Direction));

                return _mapper.Map<IList<OrganisationDTO>>(organisations);
        }

        public async Task<OrganisationDTO> GetOrganisationByIdAsync(int organisationId)
        {
            var organisation = await _organisationRepository.GetByIdAsync(organisationId);

            if (organisation is null)
            {
                throw new ApiException($"Organisation: {organisationId} not found", 404);
            }

            return _mapper.Map<OrganisationDTO>(organisation);
        }

        public async Task CreateOrganisationAsync(CreateOrganisationDTO createOrganisationDTO)
        {
            var organisation = _mapper.Map<Organisation>(createOrganisationDTO);

            await _organisationRepository.AddAsync(organisation);
        }

        public async Task UpdateOrganisationAsync(UpdateOrganisationDTO updateOrganisationDTO)
        {
            var organisation = await _organisationRepository.GetByIdAsync(updateOrganisationDTO.Id);

            if (organisation is null)
            {
                throw new ApiException($"Organisation {updateOrganisationDTO.Id} not found", 404);
            }

            _mapper.Map(updateOrganisationDTO, organisation);

            await _organisationRepository.UpdateAsync(organisation);
        }

        public async Task DeleteOrganisationAsync(int organisationId)
        {
            var organisation = await _organisationRepository.GetByIdAsync(organisationId);

            if (organisation is null)
            {
                throw new ApiException($"Organisation {organisationId} not found", 404);
            }

            await _organisationRepository.DeleteAsync(organisation);
        }
    
    }
}
