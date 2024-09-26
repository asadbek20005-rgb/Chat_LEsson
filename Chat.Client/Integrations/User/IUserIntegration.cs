using Chat.Client.DTOs.User;
using Chat.Client.Models;
using System.Net;

namespace Chat.Client.Integrations.User
{
    public interface IUserIntegration
    {
        Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser);
        Task<Tuple<HttpStatusCode, object>> Register(RegisterUserModel loginUser);
        Task<Tuple<HttpStatusCode,List<UserDto>>> GetUsers();
    }
}
