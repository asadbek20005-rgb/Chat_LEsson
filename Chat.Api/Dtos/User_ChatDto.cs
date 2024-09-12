using Chat.Api.Entities;

namespace Chat.Api.Dtos
{
    public class User_ChatDto
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid ChatId { get; set; }

    }
}
