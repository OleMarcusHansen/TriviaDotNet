using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HIOF.Net.V2023.Notification
{
    [Authorize]
    public class NotificationHub : Hub
    {
        /*public async Task SendNotification(string user, string message)
        {
            Console.WriteLine("sendNoti");
            Console.WriteLine(message);
            await Clients.All.SendAsync("Notify", user, message);
        }*/

    }
}
