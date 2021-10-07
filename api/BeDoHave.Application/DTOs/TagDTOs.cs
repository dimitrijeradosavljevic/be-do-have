using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeDoHave.Application.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateTagDto
    {
        public string Name { get; set; }
    }

    public class UpdateTagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
