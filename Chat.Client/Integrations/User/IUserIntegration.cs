using Chat.Client.DTOs.User;
using Chat.Client.Models.User;
using Microsoft.AspNetCore.Http.Internal;
using System.Net;

namespace Chat.Client.Integrations.User
{
    public interface IUserIntegration   
    {
        Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser);
        Task<Tuple<HttpStatusCode, object>> Register(RegisterUserModel loginUser);
        Task<Tuple<HttpStatusCode,List<UserDto>>> GetUsers();
        Task<Tuple<HttpStatusCode, UserDto>> GetProfile();
        Task<Tuple<HttpStatusCode, UserDto>> UpdateProfile(UpdateProfileModel profileModel);
        Task<Tuple<HttpStatusCode, byte[]>> AddOrUpdatePhoto(FormFile fileClass);
    }
}
