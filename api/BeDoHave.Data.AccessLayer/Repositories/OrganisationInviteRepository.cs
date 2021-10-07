using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Entities;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class OrganisationInviteRepository: AsyncRepository<OrganisationInvite>, IOrganisationInviteRepository
    {
        
        public OrganisationInviteRepository(DocumentDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<PaginationResponse<OrganisationInviteDb>> GetInvitesAsync(PaginationParameters parameters)
        {
            var invites = await _dbContext.OrganisationInvites.Select(oi => new OrganisationInviteDb()
                                                                    {
                                                                        Id = oi.Id,
                                                                        OrganisationId = oi.OrganisationId,
                                                                        InviterId = oi.InviterId,
                                                                        InvitedId = oi.InvitedId,
                                                                        OrganisationName = oi.Organisation.Name,
                                                                        InviterName = oi.Inviter.FullName
                                                                    })
                                                                    .Where(oi => oi.InvitedId == parameters.UserId)
                                                                    .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                                                                    .Take(parameters.PageSize)
                                                                    .OrderBy(oi => oi.Id)
                                                                    .ToListAsync();

            var total = await _dbContext.OrganisationInvites.Where(oi => oi.InvitedId == parameters.UserId)
                                                            .CountAsync();

            
            return new PaginationResponse<OrganisationInviteDb>()
            {
                Items = invites,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                Total = total
            };
        }

        public async Task<Organisation> RespondOnInviteAsync(int inviteId, bool response)
        {
            var invite = await this.GetByIdAsync(inviteId);
            Organisation organisation = null;

            invite.Accepted = response;
            _dbContext.OrganisationInvites.Update(invite);

            if (response)
            {
                organisation = await _dbContext.Organisations.FindAsync(invite.OrganisationId);
                organisation.OrganisationMembers = new List<OrganisationMember>();
                organisation.OrganisationMembers.Add(
                    new OrganisationMember()
                    {
                        OrganisationId = invite.OrganisationId,
                        MemberId = invite.InvitedId
                    });
                
                
                _dbContext.Organisations.Update(organisation);
            }
            
            await _dbContext.SaveChangesAsync();
            return organisation;
        }
    }
}