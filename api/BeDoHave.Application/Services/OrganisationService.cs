using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Data.AccessLayer.Interfaces;
using AutoMapper;
using BeDoHave.Application.Interfaces;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Specifications;
using BeDoHave.Shared.Entities;
using BeDoHave.Shared.Exceptions;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using System;
using System.Linq;
using BeDoHave.Application.Dtos;

namespace BeDoHave.Application.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IOrganisationProcedureRepository _organisationProcedureRepository;
        private readonly IMapper _mapper;

        public OrganisationService(
            IOrganisationRepository organisationRepository,
            IOrganisationProcedureRepository organisationProcedureRepository,
            IMapper mapper)
        {
            _organisationRepository = organisationRepository;
            _organisationProcedureRepository = organisationProcedureRepository;
            _mapper = mapper;
        }

        public async Task<IList<OrganisationDTO>> GetAllOrganisationsAsync(int userId)
        {
            var organisations = await _organisationRepository.GetAllOrganisationsAsync(userId);

            return _mapper.Map<IList<OrganisationDTO>>(organisations);
        }

        public async Task<PaginationResponse<OrganisationDTO>> GetOrganisationsAsync(int userId, PaginationParameters paginationParameters)
        {
            var response = await _organisationRepository.GetUserOrganisationsAsync(userId, paginationParameters);
            
                
            return new PaginationResponse<OrganisationDTO>()
            {
                Items = _mapper.Map<ICollection<OrganisationDTO>>(response.Items),
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total
            };
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

        public async Task<PaginationResponse<UserDto>> GetOrganisationMembersAsync(int organisationId, PaginationParameters paginationParameters)
        {
            var response = await _organisationRepository.GetMembersAsync(organisationId, paginationParameters);

            return new PaginationResponse<UserDto>()
            {
                Items = _mapper.Map<ICollection<UserDto>>(response.Items),
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total
            };
        }

        public async Task<PaginationResponse<UserDto>> GetOrganisationNonMembersAsync(int organisationId, PaginationParameters paginationParameters)
        {
            var response = await _organisationRepository.GetNonMembersAsync(organisationId, paginationParameters);

            return new PaginationResponse<UserDto>()
            {
                Items = _mapper.Map<ICollection<UserDto>>(response.Items),
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total
            };
        }

        public async Task AddMemberAsync(int organisationId, int userId)
        {
            await _organisationRepository.AddMemberAsync(organisationId, userId);
        }

        public async Task<OrganisationDTO> CreateOrganisationAsync(CreateOrganisationDTO createOrganisationDTO)
        {
            var organisation = _mapper.Map<Organisation>(createOrganisationDTO);

            organisation.OrganisationMembers = new List<OrganisationMember>()
            {
                new OrganisationMember()
                {
                    MemberId = createOrganisationDTO.AuthorId
                }
            };

            await _organisationRepository.AddAsync(organisation);

            return _mapper.Map<OrganisationDTO>(organisation);
        }

        public async Task<OrganisationDTO> UpdateOrganisationAsync(UpdateOrganisationDTO updateOrganisationDTO)
        {
            var organisation = await _organisationRepository.GetByIdAsync(updateOrganisationDTO.Id);

            if (organisation is null)
            {
                throw new ApiException($"Organisation {updateOrganisationDTO.Id} not found", 404);
            }

            _mapper.Map(updateOrganisationDTO, organisation);

            await _organisationRepository.UpdateAsync(organisation);

            return _mapper.Map<OrganisationDTO>(organisation);
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

        public async Task<PageTreeDTO> GetOrganisationTreeAsync(int organisationId)
        {
            var organisation = await _organisationRepository.GetByIdAsync(organisationId);

            if (organisation is null)
            {
                throw new ApiException($"Organisation {organisationId} not found", 404);
            }

            var tree = await _organisationProcedureRepository.GetOrganisationTreeAsync(organisationId);

            return MakeATree(organisationId, true, tree);
        }

        private PageTreeDTO MakeATree(int rootId, bool root, IList<PageTree> nodes)
        {
            PageTree pageTree = nodes.Where(node => node.RootId == rootId && node.QueryRoot == root).First();

            var pageTreeDto = new PageTreeDTO();
            pageTreeDto.Id = rootId;
            pageTreeDto.Title = pageTree.Title;
            pageTreeDto.IconName = pageTree.IconName;
            pageTreeDto.IconColor = pageTree.IconColor;
            pageTreeDto.Descedants = new List<PageTreeDTO>();

            string[] children = pageTree.Children.Split('-').Where(val => val != "").ToArray();
            foreach (string child in children)
            {
                pageTreeDto.Descedants.Add(MakeATree(Int32.Parse(child), false, nodes));
            }

            return pageTreeDto;
        }
    }
}
