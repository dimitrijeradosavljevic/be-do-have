using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeDoHave.Data.Core.Entities
{
    public class Organisation: BaseEntity
    {
        public string Name { get; set; }

        public int AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        
        public IList<Page> Pages { get; set; }
        
        public virtual ICollection<User> Members { get; set; }
        public ICollection<OrganisationMember> OrganisationMembers { get; set; }
        
        public ICollection<OrganisationInvite> OrganisationInvites { get; set; }
    }
}
