using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using BeDoHave.Data.Core.Entities;
using BeDoHave.ElasticSearch.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Services
{
    public class TagService: ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(
            IMapper mapper,
            ITagRepository tagRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
        }
        
        public async Task<PaginationResponse<TagDto>> GetTagsAsync(PaginationParameters paginationParameters)
        {
            var response = await _tagRepository.GetTagsAsync(paginationParameters);
            
            return new PaginationResponse<TagDto>()
            {
                Items = _mapper.Map<ICollection<TagDto>>(response.Items),
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total
            };
        }

        public async Task<TagDto> CreateTagAsync(CreateTagDto createTagDto)
        {
            var tag = _mapper.Map<Tag>(createTagDto);
            
            await _tagRepository.AddAsync(tag);

            return _mapper.Map<TagDto>(tag);
        }

        public async Task<IList<TagWeight>> GetTagsHierarchieAsync(int organisationId, IList<int> tagIds)
        {
            var tagsWeights = new List<TagWeight>();
            foreach (var tagId in tagIds)
            {
                tagsWeights.Add(new TagWeight()
                {
                    Id = tagId,
                    Weight = 1
                });
            }

            return await _tagRepository.GetTagsHierarchieAsync(organisationId, tagsWeights);
        }
    }
}