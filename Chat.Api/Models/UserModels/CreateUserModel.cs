using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Models
{
    public class CreateUserModel
    {
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        public byte? Age { get; set; }
    }
}
