using System.ComponentModel.DataAnnotations;

namespace Chat.Client.Models.User
{
    public class LoginUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
