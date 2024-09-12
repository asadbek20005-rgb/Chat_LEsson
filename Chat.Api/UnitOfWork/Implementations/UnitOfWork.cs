using Chat.Api.Context;
using Chat.Api.UnitOfWork.Implementations;
using Chat.Api.UnitOfWork.Interfaces;

namespace Chat.Api.UnitOfWork.Classes
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed=false;
        public IUserRepository _UserRepository;
        public IChatRepository _ChatRepository;
        public IUserChatRepository _UserChatRepository;
        public IMessageRepository _MessageRepository;   


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }



        public IUserRepository UserRepository
        {
            get
            {
                if(_UserRepository == null)
                {
                    _UserRepository = new UserRepository(_context);
                }
                return _UserRepository;
            }
        }
        public IChatRepository ChatRepository
        {
            get
            {
                if (_ChatRepository == null)
                    _ChatRepository = new ChatRepository(_context);
                return _ChatRepository;
            }
        }

        public IUserChatRepository UserChatRepository
        {
            get
            {
                if(_UserChatRepository == null)
                    _UserChatRepository = new UserChatRepository(_context);
                return _UserChatRepository;
            }
        }

        public IMessageRepository MessageRepository => new MessageRepository(_context);

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();

                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
