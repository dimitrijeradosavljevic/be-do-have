using AutoMapper;
using BeDoHave.Application.Dtos;
using BeDoHave.Data.Core.Entities;
using BeDoHave.ElasticSearch.Entities;

namespace BeDoHave.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, UserWithEmailDto>();

            CreateMap<User, Author>();
        }
    }
}
