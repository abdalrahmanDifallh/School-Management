using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Notification
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
    }
}
