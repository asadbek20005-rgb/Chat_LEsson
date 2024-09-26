using Chat.Client.DTOs.Message;
using System.Net;

namespace Chat.Client.Integrations.Message
{
    public interface IMessageIntegration
    {
        Task<Tuple<HttpStatusCode, List<MessageDto>>> GetChatMessages(Guid chatId);
    }
}
