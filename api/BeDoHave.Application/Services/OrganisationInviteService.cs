using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Services
{
    public class OrganisationInviteService: IOrganisationInviteService
    {
        private readonly IOrganisationInviteRepository _organisationInviteRepository;
        private readonly IMapper _mapper;

        public OrganisationInviteService(
            IOrganisationInviteRepository organisationInviteRepository,
            IMapper mapper)
        {
            _organisationInviteRepository = organisationInviteRepository;
            _mapper = mapper;
        }
        
        public async Task InviteAsync(int organisationId, CreateOrganisationInviteDto crateInviteDto)
        {
            var invite = _mapper.Map<OrganisationInvite>(crateInviteDto);
            invite.OrganisationId = organisationId;
            
            await _organisationInviteRepository.AddAsync(invite);
        }

        public async Task<PaginationResponse<OrganisationInviteDto>> GetInvitesAsync(PaginationParameters parameters)
        {
            var response = await _organisationInviteRepository.GetInvitesAsync(parameters);
            
            return new PaginationResponse<OrganisationInviteDto>()
            {
                Items = _mapper.Map<ICollection<OrganisationInviteDto>>(response.Items),
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total
            };
        }

        public async Task<OrganisationDTO> RespondOnInviteAsync(int inviteId, bool response)
        {
            var result = await _organisationInviteRepository.RespondOnInviteAsync(inviteId, response);

            if (result is not null)
            {
                return _mapper.Map<OrganisationDTO>(result);
            }

            return null;
        }
    }
}