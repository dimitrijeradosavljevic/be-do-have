using System;
using System.Collections.Generic;
using System.Text;

namespace BeDoHave.Data.Core.Entities
{
    public class Tag: BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public IList<TagPage> TagPages { get; set; }
    }
}
