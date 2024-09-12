using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Entities
{
    public class User 
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Gender { get; set; }
        public string? Bio {  get; set; }
        public byte[]? PhotoData { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedDate => DateTime.UtcNow;
        public byte? Age { get; set; }
        [Required]
        public string Role { get; set; }
       

        public virtual List<User_Chat>? User_Chats { get; set; }
    }
}
