using Blazored.LocalStorage;
using Chat.Client.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.Client.BlazorCustomAuth
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService _storageService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly HttpClient _httpClient;
        public CustomAuthProvider(LocalStorageService localStorageService, HttpClient httpClient)
        {
            _storageService = localStorageService;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _httpClient = httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storageService.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                var result = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(result);

            }


            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuth");
            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
            return new AuthenticationState(principal);
        }


        private List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(jwt);
            var username = jwtSecurityToken.Claims
                     .FirstOrDefault(c =>
                  c.Type == ClaimTypes.GivenName
                     )!.Value;

            var role = jwtSecurityToken.Claims
                     .FirstOrDefault(c =>
                     c.Type == ClaimTypes.Role)!.Value;

            var userId = jwtSecurityToken.Claims
                     .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var claims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, userId),
               new Claim(ClaimTypes.GivenName, username),
               new Claim(ClaimTypes.Role, role),

            };

            return claims;
        }
    }
}
