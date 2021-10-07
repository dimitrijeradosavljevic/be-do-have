using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface ITagRepository: IAsyncRepository<Tag>
    {
        Task<PaginationResponse<Tag>> GetTagsAsync(PaginationParameters paginationParameters);
        Task<IList<TagWeight>> GetTagsHierarchieAsync(int organisationId, IList<TagWeight> tagIds);

        Task AddDefaultTagAsync(int pageId);
    }
}