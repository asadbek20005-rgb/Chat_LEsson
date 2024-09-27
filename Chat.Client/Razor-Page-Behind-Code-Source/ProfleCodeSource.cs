using Chat.Client.DTOs.User;
using Chat.Client.Integrations.User;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class ProfleCodeSource : ComponentBase
    {
        [Inject]
        protected IUserIntegration UserIntegration { get; set; }
        protected UserDto UserDto { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            UserDto = await GetProfile();
        }

        protected async Task<UserDto> GetProfile()
        {
            var (statusCode, userDto) = await UserIntegration.GetProfile();
            if (statusCode == System.Net.HttpStatusCode.OK)
                return userDto;
            else NavigationManager.NavigateTo($"/home/{statusCode}");
            return new UserDto();
        }
    }
}