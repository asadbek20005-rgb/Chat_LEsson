using Chat.Api.Context;
using Chat.Api.Entities;
using Chat.Api.UnitOfWork.Interfaces;

namespace Chat.Api.UnitOfWork.Implementations
{
    public class UserChatRepository : IUserChatRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;
        public UserChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserChat(User_Chat user_Chat)
        {
            await _context.User_Chats.AddAsync(user_Chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserChat(User_Chat user_Chat)
        {
            _context.User_Chats.Remove(user_Chat);
            await _context.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
