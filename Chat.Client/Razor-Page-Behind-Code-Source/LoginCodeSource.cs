using Chat.Client.Integrations.User;
using Chat.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class LoginCodeSource : ComponentBase
    {
        [Inject]
        protected IUserIntegration UserIntegration { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected LoginUserModel LoginUserModel = new LoginUserModel();
        public async Task<string> Login()
        {
            var (statusCode, response) = await UserIntegration.Login(LoginUserModel);
            bool isOk = statusCode == System.Net.HttpStatusCode.OK;
            bool isBadRequest = statusCode == System.Net.HttpStatusCode.BadRequest;

            if (isOk)
                NavigationManager.NavigateTo($"/home/{response}");
            if (isBadRequest)
                NavigationManager.NavigateTo($"/home/{statusCode.ToString()}");
            return "Login wasn't done";
        }

        public void NavigateToRegisterPage()
        {
            NavigationManager.NavigateTo("/account/register");
        }
    }
}
