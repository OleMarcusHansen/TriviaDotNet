using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HIOF.Net.V2023.Notification
{
    [Authorize]
    public class NotificationHub : Hub
    {

    }
}
