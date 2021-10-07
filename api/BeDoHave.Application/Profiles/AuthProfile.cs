using AutoMapper;
using BeDoHave.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Application.Profiles
{
    class AuthProfile: Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, LoginDto>();
        }
    }
}
