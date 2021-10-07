using System;

namespace BeDoHave.Data.Core.Entities
{
    public class OrganisationInvite: BaseEntity
    {
        public int OrganisationId { get; set; }
        public virtual Organisation Organisation { get; set; }
        
        public int InviterId { get; set; }
        public virtual User Inviter { get; set; }
        
        public int InvitedId { get; set; }
        public virtual User Invited { get; set; }

        
        public bool? Accepted { get; set; }
    }
}