using Chat.Client.DTOs.Message;
using Chat.Client.LocalStorage;
using System.Net;
using System.Net.Http.Json;

namespace Chat.Client.Integrations.Message
{
    public class MessageIntegration : IMessageIntegration
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;
        public MessageIntegration(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }   

        public async Task<Tuple<HttpStatusCode, List<MessageDto>>> GetChatMessages(Guid chatId)
        {
            string url = $"/api/users/userId/chats/{chatId}/messages";
            var token = await _localStorageService.GetToken();
            if (!string.IsNullOrEmpty(token))   
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);

            var messageDtos = await response.Content.ReadFromJsonAsync<List<MessageDto>>();

            if (response.StatusCode == HttpStatusCode.OK)
                return new(response.StatusCode, messageDtos);
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return new(response.StatusCode, null);
            return new(response.StatusCode, messageDtos);
        }
    }
}