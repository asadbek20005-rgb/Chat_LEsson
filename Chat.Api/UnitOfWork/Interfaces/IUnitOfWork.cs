namespace Chat.Api.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IChatRepository ChatRepository { get; }
        public IUserChatRepository UserChatRepository { get; }
        public IMessageRepository MessageRepository { get; }

    }
}
