using Chat.Client.DTOs.UserChat;
using Chat.Client.LocalStorage;
using System.Net;
using System.Net.Http.Json;

namespace Chat.Client.Integrations.UserChat
{
    public class UserChatIntegration : IUserChatIntegration
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;
        public UserChatIntegration(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<Tuple<HttpStatusCode, List<MessageDto>>> GetChatMessages(Guid chatId)
        {
            await AddTokenToHeader();
            string url = $"api/users/userId/chats/{chatId}/messages";
            var response = await _httpClient.GetAsync(url);
            var messageDtos = await response.Content.ReadFromJsonAsync<List<MessageDto>>();
            if(response.StatusCode == HttpStatusCode.OK) 
                return Tuple.Create(response.StatusCode, messageDtos)!;

            return Tuple.Create(response.StatusCode, messageDtos)!;
        }

        public async Task<Tuple<HttpStatusCode, ChatDto>> GetUserChat(Guid toUserId)
        {
            await AddTokenToHeader();

            string url = $"api/users/userId/chats";
            var response = await _httpClient.PostAsJsonAsync(url, toUserId);
            var chatDto = await response.Content.ReadFromJsonAsync<ChatDto>();
            return new(response.StatusCode, chatDto);
        }

        public async Task<Tuple<HttpStatusCode, List<ChatDto>>> GetUserChats()
        {
            string url = "api/users/userId/Chats";
            await AddTokenToHeader();

            var response = await _httpClient.GetAsync(url);
            var userChats = await response.Content.ReadFromJsonAsync<List<ChatDto>>();
            return Tuple.Create(response.StatusCode, userChats)!;
        }

        public async Task<Tuple<HttpStatusCode, MessageDto>> SendMessage(Guid chatId, string text)
        {
            await AddTokenToHeader();
            string url = $"api/users/userId/chats/{chatId}/messages";
            var response = await _httpClient.PostAsJsonAsync(url, text);
            var messageDto = await response.Content.ReadFromJsonAsync<MessageDto>();
            return Tuple.Create(response.StatusCode, messageDto)!;
        }


        private async Task AddTokenToHeader()
        {
            var token = await _localStorageService.GetToken();
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
