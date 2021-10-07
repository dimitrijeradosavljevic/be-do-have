using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Application.DTOs
{
    public class PageTreeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string IconName { get; set; }
        
        public string IconColor { get; set; }
        
        [DefaultValue(false)]
        public bool Open { get; set; }
        public ICollection<PageTreeDTO> Descedants { get; set; }
    }
}
