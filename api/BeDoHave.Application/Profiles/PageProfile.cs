using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.Core.Entities;
using BeDoHave.ElasticSearch.Entities;

namespace BeDoHave.Application.Profiles
{
    class PageProfile: Profile
    {
        public PageProfile()
        {
            CreateMap<CreatePageDTO, Page>();
            CreateMap<Page, PageDTO>();
            CreateMap<Page, PageSearchDto>();
            CreateMap<UpdatePageDTO, Page>();
            CreateMap<Page, DescendantDto>();

            CreateMap<Page, PageSearch>()
                .ForMember(search => search.PageId,
                    opt => opt.MapFrom(page => page.Id))
                .ForMember(search => search.Author,
                    opt => opt.MapFrom(page => page.User));
            
            CreateMap<PageSearch, PageSearchDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(search => search.PageId));

        }
    }
}
