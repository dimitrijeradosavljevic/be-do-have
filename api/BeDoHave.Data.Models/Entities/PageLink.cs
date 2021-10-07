using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Data.Core.Entities
{
    public class PageLink: BaseEntity
    {
        public int AncestorPageId { get; set; }
        
        [ForeignKey("AncestorPageId")]
        public virtual Page AncestorPage { get; set; }
        
        public int DescendantPageId { get; set; }
        
        [ForeignKey("DescendantPageId")]
        public virtual Page DescendantPage { get; set; }
        public int Depth { get; set; }
    }
}
