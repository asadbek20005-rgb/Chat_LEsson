using Chat.Client.DTOs.User;
using Chat.Client.LocalStorage;
using Chat.Client.Models.User;
using System.Net;
using System.Net.Http.Json;

namespace Chat.Client.Integrations.User
{
    public class UserIntegration : IUserIntegration
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _storageService;
        public UserIntegration(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _storageService = localStorageService;
        }

        public async Task<Tuple<HttpStatusCode, UserDto?>> GetProfile()
        {
            string url = "/api/users/profile";
            string token = await _storageService.GetToken();
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);
            var profile = await response.Content.ReadFromJsonAsync<UserDto>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return Tuple.Create(HttpStatusCode.OK, profile);
            return new(response.StatusCode, profile);
        }

        public async Task<Tuple<HttpStatusCode, List<UserDto>>> GetUsers()
        {
            string url = "/api/users";
            var token = await _storageService.GetToken();
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);
            var userDtos = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            if (response.StatusCode == HttpStatusCode.OK)
                return new(HttpStatusCode.OK, userDtos);
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                return new(HttpStatusCode.Unauthorized, userDtos);
            else return new(response.StatusCode, userDtos);
        }

        public async Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser)
        {
            string url = "/api/Users/Login";
            var response = await _httpClient.PostAsJsonAsync(url, loginUser);
            string token = await response.Content.ReadAsStringAsync();
            await _storageService.SetToken(token);
            return new(response.StatusCode, token);
        }

        public async Task<Tuple<HttpStatusCode, object>> Register(RegisterUserModel registerUser)
        {
            string url = "/api/Users/Register";
            var response = await _httpClient.PostAsJsonAsync(url, registerUser);
            var result = await response.Content.ReadAsStringAsync();
            return new(response.StatusCode, result);
        }

        public Task<Tuple<HttpStatusCode, UserDto>> UpdateProfile()
        {

        }
    }
}