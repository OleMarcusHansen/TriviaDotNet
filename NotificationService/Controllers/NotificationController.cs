using Microsoft.AspNetCore.Mvc;
using NotificationService.Model.V1;
using System.Text.Json;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {

        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetNotificationRequest")]

        public IActionResult GetNotification(int userid)
        {
            var Notification = new Notification { Message= "Come back and play ! We have not seen you in four years :/", UserID = userid};
            string json = JsonSerializer.Serialize(Notification, new JsonSerializerOptions { WriteIndented= true });    
            return Ok(json);
        }



    }
}