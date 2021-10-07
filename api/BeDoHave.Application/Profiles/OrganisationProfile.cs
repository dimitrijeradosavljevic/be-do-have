using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Application.Profiles
{
    class OrganisationProfile: Profile
    {
        public OrganisationProfile()
        {
            CreateMap<CreateOrganisationDTO, Organisation>();
            CreateMap<Organisation, OrganisationDTO>();
            CreateMap<UpdateOrganisationDTO, Organisation>();
        }
    }
}
