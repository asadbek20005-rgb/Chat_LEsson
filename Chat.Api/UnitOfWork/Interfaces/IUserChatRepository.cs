using Chat.Api.Entities;

namespace Chat.Api.UnitOfWork.Interfaces
{
    public interface IUserChatRepository
    {
        public Task AddUserChat(User_Chat user_Chat);
        public Task DeleteUserChat(User_Chat user_Chat);
    }
}
