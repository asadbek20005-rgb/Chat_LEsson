using Chat.Client.DTOs.User;
using Chat.Client.Integrations.User;
using Chat.Client.Models.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class ProfleCodeSource : ComponentBase
    {
        [Inject]
        protected IUserIntegration UserIntegration { get; set; }
        
        protected UserDto UserDto { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        protected string ProfileImageUrl { get; set; }
        protected IBrowserFile BrowserFile { get; set; }
        private FileClass FileClass { get; set; } = new FileClass();
        protected InputFile InputFile { get; set; }

        protected string PhotoUrl { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }
        protected override async Task OnInitializedAsync()
        {
            UserDto = await GetProfile();
            if (UserDto.PhotoData == null)
                PhotoUrl = "https://i.imgur.com/wvxPV9S.png";
            else
            {
                string imageBase64 = Convert.ToBase64String(UserDto.PhotoData!);
                PhotoUrl = $"data:image/jpeg;base64,{imageBase64}";
            }
            // ProfileImageUrl = await UploadFile();

        }

        protected async Task<UserDto> GetProfile()
        {
            var (statusCode, userDto) = await UserIntegration.GetProfile();
            if (statusCode == System.Net.HttpStatusCode.OK)
                return userDto;
            else NavigationManager.NavigateTo($"/home/{statusCode}");
            return new UserDto();
        }

        protected  void HandleSelected(InputFileChangeEventArgs fileClass)
        {
            BrowserFile = fileClass.GetMultipleFiles().FirstOrDefault();
            //using var memoryStream = new MemoryStream();
            //await BrowserFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memoryStream);
            ////if (fileClass.File.Size != 0)
            ////{
            ////var formFile = new FormFile(new MemoryStream(memoryStream.ToArray()), 0, memoryStream.Length, BrowserFile.Name, BrowserFile.Name)
            ////{
            ////    ContentType = BrowserFile.ContentType
            ////};

            //FormFile.OpenReadStream();
            //var (statusCode, bytes) = await UserIntegration.AddOrUpdatePhoto(FormFile);
            //if (statusCode == System.Net.HttpStatusCode.OK)
            //{
            //    string imageBase64 = Convert.ToBase64String(bytes);
            //    string profileImageUrl = $"data:image/jpeg;base64,{imageBase64}";
            //    ProfileImageUrl = profileImageUrl;
            //}

            //NavigationManager.NavigateTo($"/home/{statusCode}");
            ////}
        }


        protected async Task UploadFile(InputFileChangeEventArgs fileClass)
        {
            BrowserFile = fileClass.GetMultipleFiles().FirstOrDefault();
            var content = new MultipartFormDataContent();
            var stream = BrowserFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            content.Add(new StreamContent(stream), "fileClass", BrowserFile.Name);


            var response = await HttpClient.PostAsync("/api/users/userId/add-or-update-photo", content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK) //userId/add-or-update-photo
                ProfileImageUrl += response.Content.ReadAsStringAsync();
            NavigationManager.Refresh(true);
        }


        protected void GoToUptdaePage()
        {
            NavigationManager.NavigateTo($"/account/profile/update");
        }
    }
}