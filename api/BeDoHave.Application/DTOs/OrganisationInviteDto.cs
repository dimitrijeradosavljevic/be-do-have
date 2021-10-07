using System.ComponentModel.DataAnnotations;

namespace BeDoHave.Application.DTOs
{

    public class OrganisationInviteDto
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int InviterId { get; set; }
        public string InviterName { get; set; }
    }
    
    public class CreateOrganisationInviteDto
    {
        [Required]
        public int InviterId { get; set; }

        [Required]
        public int InvitedId { get; set; }
    }

}