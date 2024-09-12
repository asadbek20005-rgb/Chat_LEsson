using Chat.Api.Entities;

namespace Chat.Api.Dtos
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public List<string> ChatNames { get; set; }

        public List<User_ChatDto> User_Chats { get; set; }

        public List<MessageDto> Messages { get; set; }
    }
}
