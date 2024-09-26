using Chat.Client.BlazorCustomAuth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Chat.Client.Helper
{
    public class UserHelper
    {
        private readonly AuthenticationStateProvider _stateProvider;
        public UserHelper(AuthenticationStateProvider authenticationStateProvider)
        {
            _stateProvider = (CustomAuthProvider)authenticationStateProvider;
        }

        public async Task<string> GetUserId()
        {
            var user = await _stateProvider.GetAuthenticationStateAsync();
            var userId = user.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            return userId;
        }
    }
}
