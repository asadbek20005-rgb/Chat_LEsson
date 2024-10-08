using Chat.Client.DTOs.User;
using Chat.Client.Integrations.User;
using Chat.Client.Models.User;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class UpdateProfileCodeSource : ComponentBase
    {
        [Inject]
        IUserIntegration UserIntegration { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        protected UpdateProfileModel Model { get; set; } = new();
        protected UserDto? UserDto { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
            UserDto = await GetUserDto();
            Model = new UpdateProfileModel()
            {
                Age = UserDto!.Age.ToString(),
                FirstName = UserDto.FirstName,
                LastName = UserDto.LastName,
                Gender = UserDto.Gender,
            };
        }

        public async Task UpdateProfile()   
        {
            var (statusCode, userDto) = await UserIntegration.UpdateProfile(Model!);
            if(statusCode == System.Net.HttpStatusCode.OK)
            {
                 UserDto = userDto;
            }
        }

        private async Task<UserDto?> GetUserDto()
        {
            var (statuscode, userDto) = await UserIntegration.GetProfile();
            if (statuscode == System.Net.HttpStatusCode.OK)
            {
                return userDto;
            }
            return new UserDto();           
        }
    }
}
