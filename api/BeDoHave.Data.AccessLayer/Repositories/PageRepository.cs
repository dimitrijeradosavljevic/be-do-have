using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class PageRepository: AsyncRepository<Page>, IPageRepository
    {
        public PageRepository(DocumentDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<ICollection<Page>> GetByIdsAsync(IList<int> ids)
        {
            return await _dbContext.Pages.Where(page => ids.Contains(page.Id)).ToListAsync();
        }

        public async Task<PaginationResponse<Page>> GetPagesForPickerAsync(PaginationParameters parameters, IList<int> organisationsIds)
        {
            var pages = await _dbContext.Pages
                .Select(page => new {page.Id, page.Title, page.OrganisationId})
                .Where(page => page.Title.Contains(parameters.Keyword) && organisationsIds.Contains(page.OrganisationId.Value))
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .OrderBy(p => p.Id)
                .ToListAsync();

            var result = new List<Page>();
            foreach (var page in pages)
            {
                result.Add(new Page()
                {
                    Id = page.Id,
                    Title = page.Title
                });
            }

            var total = await _dbContext.Pages.Where(page =>
                    page.Title.Contains(parameters.Keyword) && organisationsIds.Contains(page.OrganisationId.Value)).CountAsync();

            return new PaginationResponse<Page>()
            {
                Items = result,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
        }

        public async Task TrashPageAsync(Page page, bool archived)
        {
            page.Archived = archived;
            _dbContext.Pages.Update(page);
            foreach (var descendant in page.Descendants)
            {
                descendant.Archived = archived;
            }
            _dbContext.Pages.UpdateRange(page.Descendants);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePageAsync(Page page, bool updateTitle)
        {
            var i = 0;
            foreach (var tagPage in page.TagPages)
            {
                if (!page.Tags.Select(t => t.Id).Contains(tagPage.TagId))
                {
                    _dbContext.Remove(tagPage);
                }
            }
            foreach (var tag in page.Tags)
            {
                if (!page.TagPages.Select(t => t.TagId).Contains(tag.Id))
                {
                    page.TagPages.Add(new TagPage()
                    {
                        TagId = tag.Id
                    });
                }
            }

            _dbContext.Pages.Update(page);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LoadUserAsync(Page page)
        {
            await _dbContext.Entry(page).Reference(p => p.User).LoadAsync();
        }

        public async Task MovePageAsync(int pageId, int directParentId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> 
            {    
                new SqlParameter { ParameterName = "@PageId", Value = pageId },
                new SqlParameter { ParameterName = "@DirectParentId", Value = directParentId}
            };
            
            await _dbContext.Pages.FromSqlRaw("EXEC dbo.MovePage @PageId, @DirectParentId", parameters.ToArray()).ToListAsync();
        }
        
        public async Task<Organisation> MovePageUnderOrganisationAsync(int pageId, int organisationId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> 
            {    
                new SqlParameter { ParameterName = "@PageId", Value = pageId },
                new SqlParameter { ParameterName = "@OrganisationId", Value = organisationId}
            };
            
            return await _dbContext.Organisations.FromSqlRaw("EXEC dbo.MovePageUnderOrganisation @PageId, @OrganisationId", parameters.ToArray())
                                                 .FirstAsync();
        }

        public async Task<PaginationResponse<Page>> GetTrashedPagesAsync(PaginationParameters parameters)
        {
            var pages = await _dbContext.Pages.Where(page => page.Archived.Equals(true) &&
                                                             page.Title.Contains(parameters.Keyword) &&
                                                             page.OrganisationId == parameters.OrganisationId.Value)
                                                       // .Include(page => page.AncestorsLinks.OrderByDescending(al => al.Depth))
                                                       // .ThenInclude(al => al.AncestorPage.Title)
                                                       .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                                       .Take(parameters.PageSize)
                                                       .OrderBy(p => p.Id)
                                                       .ToListAsync();

            var total = await _dbContext.Pages.Where(page => page.Archived.Equals(true) &&
                                                             page.Title.Contains(parameters.Keyword) &&
                                                             page.OrganisationId == parameters.OrganisationId.Value)
                                                 .CountAsync();

            return new PaginationResponse<Page>()
            {
                Items = pages,
                Total = total,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize
            };
        }

        public async Task DeletePageAsync(int pageId)
        {
            await _dbContext.Pages.FromSqlRaw("EXEC dbo.DeletePageTree {0}", pageId).ToListAsync();
        }
    }
}