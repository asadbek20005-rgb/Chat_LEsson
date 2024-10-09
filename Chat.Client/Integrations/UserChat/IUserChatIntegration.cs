using Chat.Client.DTOs.UserChat;
using Chat.Client.Models.Message;
using System.Net;

namespace Chat.Client.Integrations.UserChat
{
    public interface IUserChatIntegration
    {
        Task<Tuple<HttpStatusCode, List<ChatDto>>> GetUserChats();
        Task<Tuple<HttpStatusCode, ChatDto>> GetUserChat(Guid chatId);
        Task<Tuple<HttpStatusCode, List<MessageDto>>> GetChatMessages(Guid chatId);
        Task<Tuple<HttpStatusCode, MessageDto>> SendTextMessage(Guid chatId, SendMessageModel model);
    }
}