using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using HIOF.Net.V2023.Notification;
using Microsoft.Data.SqlClient;
using HIOF.Net.V2023.Notification.Services;
using Microsoft.AspNetCore.Authorization;

namespace HIOF.Net.V2023.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationSink _notificationSink;

        public NotificationController(INotificationSink notificationSink) => _notificationSink = notificationSink;

        [Authorize]
        [HttpGet("/notify")]
        public async Task<IActionResult> Notify(string user, string message)
        {
            await _notificationSink.PushAsync(new HIOF.Net.V2023.Notification.Services.Notification(user, message));
            return Ok();
        }
    }
}