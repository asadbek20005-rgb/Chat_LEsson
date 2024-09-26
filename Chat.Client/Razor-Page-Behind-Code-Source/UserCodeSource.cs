using Chat.Client.DTOs.User;
using Chat.Client.Integrations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class UserCodeSource : ComponentBase
    {
        [Inject]
        private IUserIntegration UserIntegration { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        protected List<UserDto>? Users { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Users = await GetUsers();
        }


        public async Task<List<UserDto>> GetUsers()
        {
            var (statusCode, userDtos) = await UserIntegration.GetUsers();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                return userDtos;
            }
            else if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                NavigationManager.NavigateTo($"/home/{statusCode}");
            return null;
        }
    }
}
    