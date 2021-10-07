using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Data.Core.Entities
{
    public class OrganisationPage: BaseEntity
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
        [Required]
        public string PageTitle { get; set; }
        public bool Direct { get; set; }
    }
}
