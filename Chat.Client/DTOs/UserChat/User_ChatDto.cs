namespace Chat.Client.DTOs.UserChat
{
    public class User_ChatDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid ChatId { get; set; }
    }
}
