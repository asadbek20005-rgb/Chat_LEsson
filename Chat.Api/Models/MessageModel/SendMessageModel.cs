using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Models.MessageModel
{
    public class SendMessageModel
    {
        [Required]
        public string Text { get; set; }
        public string? FileUrl { get; set; }
        public string? Caption { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
