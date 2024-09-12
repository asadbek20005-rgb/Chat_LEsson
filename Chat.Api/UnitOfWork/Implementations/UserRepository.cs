using Chat.Api.Context;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.UnitOfWork.Classes
{
    public class UserRepository(AppDbContext context) : IUserRepository, IDisposable
    {
        private readonly AppDbContext _context = context;
        private bool _disposed = false;
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
            _context.Users.Remove(user);
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
            _disposed = true;
        }

         public void Dispose()
          {
             Dispose(true);
              GC.SuppressFinalize(this);
          }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.AsTracking().ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
        
            return user;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            return user;
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
