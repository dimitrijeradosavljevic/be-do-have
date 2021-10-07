using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeDoHave.Data.Core.Entities
{
    public class Page: BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [DefaultValue(false)]
        public bool Archived { get; set; }

        public string IconName { get; set; }
        
        public string IconColor { get; set; }
        
        public virtual List<Page> Ancestors { get; set; } = new List<Page>();
        public IList<PageLink> AncestorsLinks { get; set; }
        
        public virtual List<Page> Descendants { get; set; } = new List<Page>();
        public List<PageLink> DescendantsLinks { get; set; }
        
        public int? OrganisationId { get; set; }
        
        [DefaultValue(false)]
        public bool OrganisationDirect { get; set; }
        
        [ForeignKey("OrganisationId")]
        public Organisation Organisation { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public IList<TagPage> TagPages { get; set; }
    }
}
