using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Application.DTOs;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Application.Interfaces
{
    public interface ITagService
    {
        Task<PaginationResponse<TagDto>> GetTagsAsync(PaginationParameters paginationParameters);
        Task<TagDto> CreateTagAsync(CreateTagDto createTagDto);

        Task<IList<TagWeight>> GetTagsHierarchieAsync(int organisationId, IList<int> tagIds);
    }
}