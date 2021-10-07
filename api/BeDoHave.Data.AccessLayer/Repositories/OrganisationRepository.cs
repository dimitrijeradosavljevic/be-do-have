using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class OrganisationRepository: AsyncRepository<Organisation>, IOrganisationRepository
    {
        public OrganisationRepository(DocumentDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<PaginationResponse<Organisation>> GetUserOrganisationsAsync(int userId, PaginationParameters parameters)
        {
            var organisations = await _dbContext.Organisations
                                                        .Where(organisation =>
                                                            organisation.OrganisationMembers.Any(om => om.MemberId == userId) &&
                                                            organisation.Name.Contains(parameters.Keyword))
                                                        .Include(organisation => organisation.Author)
                                                        .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                                        .Take(parameters.PageSize)
                                                        .OrderBy(p => p.Id)
                                                        .ToListAsync();

            var total = await _dbContext.Organisations
                                        .Where(organisation =>
                                            organisation.OrganisationMembers.Any(om => om.MemberId == userId) &&
                                            organisation.Name.Contains(parameters.Keyword))
                                        .CountAsync();

            return new PaginationResponse<Organisation>()
            {
                Items = organisations,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
        }

        public async Task<PaginationResponse<User>> GetMembersAsync(int organisationId, PaginationParameters parameters)
        {
            var members = await _dbContext.Users
                                        .Where(user => 
                                                user.OrganisationMembers.Any(om => om.OrganisationId == organisationId))
                                        .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                        .Take(parameters.PageSize)
                                        .OrderBy(p => p.Id)
                                        .ToListAsync();

            var total = await _dbContext.Users
                                           .Where(user =>
                                                user.OrganisationMembers.Any(om => om.OrganisationId == organisationId)).CountAsync();
            
            return new PaginationResponse<User>()
            {
                Items = members,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
        }

        public async Task<PaginationResponse<User>> GetNonMembersAsync(int organisationId, PaginationParameters parameters)
        {
            
            var users = await _dbContext.Users
                                        .Where(user =>
                                            !user.OrganisationInvites.Any(om => om.OrganisationId == organisationId && (!om.Accepted.HasValue || om.Accepted.Value == true)) &&
                                            user.FullName.Contains(parameters.Keyword) &&
                                            !user.OrganisationMembers.Select(om => om.OrganisationId).Contains(organisationId))
                                        .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                        .Take(parameters.PageSize)
                                        .OrderBy(p => p.Id)
                                        .ToListAsync();

            var total = await _dbContext.Users.Where(user =>
                !user.OrganisationMembers.Select(om => om.OrganisationId).Contains(organisationId)).CountAsync();

            
            return new PaginationResponse<User>()
            {
                Items = users,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
            
        }

        public async Task AddMemberAsync(int organisationId, int userId)
        {
            var organisation = await this.GetByIdAsync(organisationId);

            organisation.OrganisationMembers = new List<OrganisationMember>()
            {
                new OrganisationMember()
                {
                    OrganisationId = organisationId,
                    MemberId = userId
                }
            };

            _dbContext.Organisations.Update(organisation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<Organisation>> GetAllOrganisationsAsync(int userId)
        {
            return await _dbContext.Organisations
                                   .Where(organisation => organisation.OrganisationMembers
                                                                      .Any(om => om.MemberId == userId))
                                   .ToListAsync();

        }
    }
}