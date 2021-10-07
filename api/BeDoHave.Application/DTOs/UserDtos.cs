using System.ComponentModel.DataAnnotations;

namespace BeDoHave.Application.Dtos
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string IdentityId { get; set; }
        // public bool ShowMyOnlineStatus { get; set; }
    }

    public class UserWithEmailDto : UserDto 
    {
        public string Email { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        // public bool ShowMyOnlineStatus { get; set; }

    }

    public class UpdateUserDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        // public bool ShowMyOnlineStatus { get; set; }
    }
}
