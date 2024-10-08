using Chat.Client.DTOs.User;
using Chat.Client.DTOs.UserChat;
using Chat.Client.Integrations.User;
using Chat.Client.Integrations.UserChat;
using Chat.Client.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using System.Security.Claims;

namespace Chat.Client.Razor_Page_Behind_Code_Source.UserChat
{
    public class UserChatCodeSource : ComponentBase
    {
        
        public List<ChatDto>? UserChats { get; set; }
        [Inject] IUserIntegration? UserIntegration { get; set; }
        [Inject] IUserChatIntegration UserChatIntegration { get; set; }
        [Inject] LocalStorageService? LocalStorageService { get; set; }
        [Inject] AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
        protected List<UserDto>? Users { get; set; }
        protected List<MessageDto>? Messages { get; set; }
        protected UserDto? User { get; set; }
        protected ChatDto? Chat { get; set; }
        protected string? Username { get; set; }
        protected string? Text { get; set; }
        protected Guid? ChatId { get; set; }
        protected HubConnection? HubConnection { get; set; }

            
        protected override async Task OnInitializedAsync()
        {

            var (statusCodeForChats, responseForChats) = await UserChatIntegration.GetUserChats();
            if(statusCodeForChats == HttpStatusCode.OK) 
                UserChats = responseForChats;

            var (statusCode2, user) = await UserIntegration!.GetProfile();
            if (statusCode2 == HttpStatusCode.OK)
                User = user;

            var (statusCode, users) = await UserIntegration!.GetUsers();
            if (statusCode2 == HttpStatusCode.OK)
                Users = users;
              
            Username = await GetUsername();
            

            GetChatNames();





            SortContacts();
            await DisconnectHub();
            await ConnectHub();

            if (ChatId != Guid.Empty)
            {
                Chat = UserChats!.SingleOrDefault(x => x.Id == ChatId)!;
                Messages = Chat.Messages;
            }
            StateHasChanged();
        }


        private async Task DisconnectHub()
        {
            if (HubConnection != null)
                await HubConnection.StopAsync();
        }

        private async Task ConnectHub()
        {
            var token = await LocalStorageService!.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                if (HubConnection is null)
                {
                    HubConnection = new HubConnectionBuilder()
                        .WithUrl($"http://localhost:5098/chat-hub/token={token}")
                        .Build();
                }
            }


            HubConnection?.On<MessageDto>("NewMessage", model =>
            {
                Messages?.Add(model);
                StateHasChanged();
            });

            await HubConnection!.StartAsync();
        }

        private void GetChatNames()
        {
            var currentFullName = GetFullName(User?.FirstName, User?.LastName);

            foreach (var chat in UserChats!)
            {
                chat.ChatName = chat.ChatNames?.First(x => x != currentFullName);
                StateHasChanged();
            }
        }


        private string GetChatName(List<string> chatNames)
        {
            var currentFullname = GetFullName(User!.FirstName, User.LastName);

            var chatName = chatNames?.First(c => c != currentFullname);

            return chatName!;

        }

        private async Task<string> GetUsername()
        {
            var customAuthProvider = (BlazorCustomAuth.CustomAuthProvider)AuthenticationStateProvider!;
            var stateProvider = await customAuthProvider.GetAuthenticationStateAsync();
            var username = stateProvider.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName)!.Value;
            return username;
        }


        private void SortContacts()
        {
            var currentUser = Users?.SingleOrDefault(x => x.Username == Username);
            if (currentUser is not null)
                Users?.Remove(currentUser);

            foreach (var chat in UserChats!)
            {
                if (chat.User_Chats is not null)
                {
                    var userChat = chat.User_Chats[0];

                    var toUserId = userChat.UserId;
                    var toUser = Users?.SingleOrDefault(x => x.Id == toUserId);
                    if (toUser is not null)
                        Users?.Remove(toUser);
                }
            }
        }

        protected void SelectedChat(Guid chatId)
        {
            ChatId = chatId;
            Chat = UserChats!.Single(x => x.Id == ChatId);
            Messages = Chat.Messages;
            StateHasChanged();
        }

        protected async Task CreateChat(Guid toUserId)
        {
            var (statusCode, userChat) = await UserChatIntegration!.GetUserChat(toUserId);
            if (statusCode == HttpStatusCode.OK)
            {
                Chat = userChat;
                Chat.ChatName = GetChatName(userChat.ChatNames!);
                Messages = Chat.Messages;

                var toUser = Users?.SingleOrDefault(u => u.Id == toUserId);

                if (toUser is not null)
                    Users?.Remove(toUser);

                UserChats?.Add(Chat);
            }
        }

        protected async Task SendMessage()
        {

            var (statusCode, response) = await UserChatIntegration!.SendMessage(Chat!.Id, Text);

            if (statusCode == HttpStatusCode.OK)
            {
                var message = response;
                Text = string.Empty;
            }

        }

        protected async Task Pressed(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendMessage();
            }
        }

        private string GetFullName(string? firstName, string? lastName)
        {
            return $"{firstName}, {lastName}";  
        }
    }
}

// Firstname: Maruf, Lastname : Berdiev => Chatname : Maruf, Berdiev
// Firstname : Asadbek , Lastname : Shermatov => Chatname : Asadbek, Shermatov
