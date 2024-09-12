using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Entities
{
    public class User_Chat
    {
        public Guid Id { get; set; }

        public Guid ToUserId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }

        public virtual User? User { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
