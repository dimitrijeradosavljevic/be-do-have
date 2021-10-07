using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using BeDoHave.Data.Core.Entities;
using BeDoHave.ElasticSearch.Entities;
using BeDoHave.Shared.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class TagRepository: AsyncRepository<Tag>, ITagRepository
    {
        public TagRepository(DocumentDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<PaginationResponse<Tag>> GetTagsAsync(PaginationParameters parameters)
        {
            var tags = await _dbContext.Tags
                                          .Where(tag => tag.Name.Contains(parameters.Keyword) && !tag.Name.Contains("_id:"))
                                          .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                          .Take(parameters.PageSize)
                                          .OrderBy(p => p.Id)
                                          .ToListAsync();

            var total = await _dbContext.Tags
                                           .Where(tag => tag.Name.Contains(parameters.Keyword))
                                           .CountAsync();

            return new PaginationResponse<Tag>()
            {
                Items = tags,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
        }
        
        public async Task<IList<TagWeight>> GetTagsHierarchieAsync(int organisationId, IList<TagWeight> tags)
        {
            var tagWeightTableSchema = new List<SqlMetaData>(2)
            {
                new SqlMetaData("Id", SqlDbType.Int),
                new SqlMetaData("Weight", SqlDbType.Int)
            }.ToArray();
            var tagWeightTable = new List<SqlDataRecord>();
            
            foreach (var tag in tags)
            {
                var tableRow = new SqlDataRecord(tagWeightTableSchema);
                tableRow.SetInt32(0, tag.Id);
                tableRow.SetInt32(1, tag.Weight);
                tagWeightTable.Add(tableRow);                
            }

            
            List<SqlParameter> parameters = new List<SqlParameter> 
            {    
                new SqlParameter { ParameterName = "@OrganisationId", Value = organisationId },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured, 
                    TypeName = "[dbo].[IdWeightArray]", 
                    ParameterName = "@Tags", 
                    Value = tagWeightTable
                }
            };
            
            return await _dbContext.TagWeights
                                   .FromSqlRaw("EXEC dbo.SelectTagTree @OrganisationId, @Tags", parameters.ToArray())
                                   .ToListAsync();
        }

        public async Task AddDefaultTagAsync(int pageId)
        {
            var defaultTag = new Tag()
            {
                Name = "_id:" + pageId
            };

            defaultTag.TagPages = new List<TagPage>()
            {
                new TagPage()
                {
                    PageId = pageId
                }
            };

            await _dbContext.Tags.AddAsync(defaultTag);
            await _dbContext.SaveChangesAsync();
        }
    }
}