using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Api.Chat_Hub
{
    public class ChatHub : Hub
    {
        private List<Tuple<Guid, string>> ConnectionIds { get; set; } = new();
        

        public override async Task OnConnectedAsync()
        {
            var userId =  Context?.User?.Claims    
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var username = Context?.User?.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.GivenName)!.Value;

            var connectionId =Context?.ConnectionId;

            ConnectionIds.Add(new( Guid.Parse(userId!),connectionId!));
        }



        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User!.Claims
               .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var username = Context.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.GivenName)!.Value;

            var connectionId = Context.ConnectionId;

            var connection = ConnectionIds.FirstOrDefault(x => x.Item2 == connectionId);

            ConnectionIds.Remove(connection!);
        }
    }
}
