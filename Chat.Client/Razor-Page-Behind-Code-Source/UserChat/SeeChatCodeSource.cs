using Chat.Client.DTOs.Message;
using Chat.Client.DTOs.UserChat;
using Chat.Client.Integrations.UserChat;
using Chat.Client.LocalStorage;
using Chat.Client.Pages.UserChats;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using MessageDto = Chat.Client.DTOs.UserChat.MessageDto;

namespace Chat.Client.Razor_Page_Behind_Code_Source.UserChat
{
    public class SeeChatCodeSource : ComponentBase
    {
        [Inject] IUserChatIntegration ChatIntegration { get; set; }
        protected ChatDto Chat { get; set; } = new();

        protected string Text { get; set; }

        protected List<MessageDto> Messages { get; set; } = new();

        [Parameter] public Guid ToUserId { get; set; }

        private HubConnection? HubConnection { get; set; }

        [Inject] LocalStorageService StorageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await DisConnectHub();
            await ConnectToHub();

            var (statusCode, chat) = await ChatIntegration.GetUserChat(ToUserId);


            if (statusCode == HttpStatusCode.OK)
            {
                Chat = chat;

                var (statusCode1, messages) =
                    await ChatIntegration.GetChatMessages(Chat.Id);


                if (statusCode1 == HttpStatusCode.OK)
                {
                    Messages = messages;
                }

            }




        }


        protected async Task SendMessage()
        {

            var (statusCode, messages) = await ChatIntegration.SendMessage(Chat.Id, Text);

            if (statusCode == HttpStatusCode.OK)
            {
                var message = messages;
                Text = string.Empty;
            }

        }


        private async Task ConnectToHub()
        {
            var token = await StorageService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                if (HubConnection == null)
                {
                    HubConnection = new HubConnectionBuilder()
                        .WithUrl($"http://localhost:5068/chat-hub?token={token}")
                        .Build();
                }
            }


            HubConnection?.On<MessageDto>("NewMessage", model =>
            {
                Messages.Add(model);
                StateHasChanged();
            });

            await HubConnection!.StartAsync();


        }

        private async Task DisConnectHub()
        {
            if (HubConnection is not null)
                await HubConnection.StopAsync();
        }
    }
}
