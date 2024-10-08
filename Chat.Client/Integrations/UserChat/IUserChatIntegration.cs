using Chat.Client.DTOs.UserChat;
using System.Net;

namespace Chat.Client.Integrations.UserChat
{
    public interface IUserChatIntegration
    {
        Task<Tuple<HttpStatusCode, List<ChatDto>>> GetUserChats();
        Task<Tuple<HttpStatusCode, ChatDto>> GetUserChat(Guid chatId);
        Task<Tuple<HttpStatusCode, List<MessageDto>>> GetChatMessages(Guid chatId);
        Task<Tuple<HttpStatusCode, MessageDto>> SendMessage(Guid chatId, string? text);
    }
}