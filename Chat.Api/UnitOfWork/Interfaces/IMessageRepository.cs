using Chat.Api.Entities;

namespace Chat.Api.UnitOfWork.Interfaces
{
    public interface IMessageRepository
    {

        Task<List<Message>> GetAllMessages(); // Admin
        Task<Message> GetMessageById(int messageId); // Admin
        Task<List<Message>> GetAllChatMessages(Guid chatId); // User
        Task<Message> GetChatMessageById(Guid chatId,int messageid); // User
        Task AddMessage(Message message); // User
    }
}
