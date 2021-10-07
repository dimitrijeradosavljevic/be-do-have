using System.ComponentModel.DataAnnotations.Schema;

namespace BeDoHave.Data.Core.Entities
{
    public class OrganisationMember: BaseEntity
    {
        
        public int OrganisationId { get; set; }
        public virtual Organisation Organisation { get; set; }
        public int MemberId { get; set; }
        
        [ForeignKey("MemberId")]
        public virtual User Member { get; set; }
        
    }
}