using BeDoHave.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeDoHave.Application.DTOs
{
    public class OrganisationDTO: Organisation
    {
    }

    public class CreateOrganisationDTO
    {
        [Required]
        public string Name { get; set; }

        public int AuthorId { get; set; }
    }

    public class UpdateOrganisationDTO
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
