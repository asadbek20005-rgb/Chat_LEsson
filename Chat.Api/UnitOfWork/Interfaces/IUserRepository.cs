using Chat.Api.Entities;

namespace Chat.Api.UnitOfWork.Interfaces
{
    public interface IUserRepository 
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUserById(Guid userId);
        public Task<User?> GetUserByUsername(string username);
        public Task AddUser(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(Guid userId);
    }
}
