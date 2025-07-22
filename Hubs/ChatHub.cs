using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{

    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);

        Task<string> GetMessage();
    }

    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        public async Task SendMessageToCaller(string user, string message)
        {
            await Clients.Caller.ReceiveMessage(user, message);
        }

        public async Task SendMessageToGroup(string user, string message)
        {
            await Clients.Group("SignalR Users").ReceiveMessage(user, message);
        }

        public async Task<string> WaitForMessage(string connectionId)
        {
            string message = await Clients.Client(connectionId).GetMessage();
            return message;
        }
    }
}