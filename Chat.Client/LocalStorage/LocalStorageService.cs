using Blazored.LocalStorage;

namespace Chat.Client.LocalStorage
{
    public class LocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private const string _key = "token";
        public LocalStorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string> GetToken()
        {
            var token = await _localStorageService.GetItemAsync<string>(_key);
            if(!string.IsNullOrEmpty(token))
            {
                return token;
            }
            return string.Empty;
        }

        public async Task SetToken(string token)
        {
            await _localStorageService.SetItemAsync(_key, token);
        }

        public async Task RemoveToken()
        {
           await _localStorageService.RemoveItemAsync(_key);
        }

    }
}