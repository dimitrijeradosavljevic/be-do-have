using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.AccessLayer.Entities;
using BeDoHave.Data.Core.Entities;

namespace BeDoHave.Application.Profiles
{
    public class OrganisationInviteProfile: Profile
    {
        public OrganisationInviteProfile()
        {
            CreateMap<CreateOrganisationInviteDto, OrganisationInvite>();
            CreateMap<OrganisationInviteDb, OrganisationInviteDto>();
        }

    }
}