using Chat.Client.Integrations.User;
using Chat.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class RegisterCodeSource : ComponentBase
    {
        [Inject]
        private IUserIntegration UserIntegration { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public RegisterUserModel RegisterUserModel = new RegisterUserModel();
        public async Task Register()
        {
            var (statusCode, response) = await UserIntegration.Register(RegisterUserModel);
            if(statusCode == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/account/login");
            }
            else
            {
                NavigationManager.NavigateTo($"/home/{response.ToString()}");
            }
        }

        public async Task NavigateToLoginPage()
        {
            NavigationManager.NavigateTo("/account/login");
        }
    }
}
