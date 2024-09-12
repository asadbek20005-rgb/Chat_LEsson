using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Models.UserModel
{
    public class LoginUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
