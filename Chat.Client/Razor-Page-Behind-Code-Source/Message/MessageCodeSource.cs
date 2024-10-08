using Chat.Client.DTOs.Message;
using Chat.Client.Integrations.Message;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Razor_Page_Behind_Code_Source
{
    public class MessageCodeSource : ComponentBase
    {
        [Inject]
        private IMessageIntegration MessageIntegration { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        protected List<MessageDto> Messages = new List<MessageDto>();
        protected Guid ChatId { get; set; }

        protected async Task GetChatMessages()
        {
            var (statusCode, messageDtos) = await MessageIntegration.GetChatMessages(ChatId);
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                Messages = messageDtos;
                NavigationManager.NavigateTo($"/messages");
            }
        
        }
    }
}
