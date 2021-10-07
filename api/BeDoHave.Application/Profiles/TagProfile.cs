using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.Core.Entities;

namespace BeDoHave.Application.Profiles
{
    public class TagProfile: Profile
    {
        public TagProfile()
        {
            CreateMap<CreateTagDto, Tag>();
            CreateMap<UpdateTagDto, Tag>();
            CreateMap<Tag, TagDto>();
        }
    }
}