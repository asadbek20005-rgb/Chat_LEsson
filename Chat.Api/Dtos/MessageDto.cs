﻿using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [Required]
        public string FromUsername { get; set; }

        public Guid FromUserId { get; set; }

        public int ContentId { get; set; }
        public ContentDto? Content { get; set; }

        public Guid ChatId { get; set; }


        public DateTime SendAt => DateTime.UtcNow;
        public DateTime? EditedAt { get; set; }
        public bool IsEdited { get; set; }
    }
}
