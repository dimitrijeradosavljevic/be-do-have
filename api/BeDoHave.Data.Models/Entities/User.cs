using System.Collections.Generic;

namespace BeDoHave.Data.Core.Entities
{
    public class User: BaseEntity
    {
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        
        public List<Page> Pages { get; set; }

        public virtual ICollection<Organisation> OrganisationsAuthor { get; set; }

        public virtual ICollection<Organisation> Organisations { get; set; }
        public ICollection<OrganisationMember> OrganisationMembers { get; set; }


        public ICollection<OrganisationInvite> OrganisationInvites { get; set; }
        public ICollection<OrganisationInvite> OrganisationInviters { get; set; }
    }
}
