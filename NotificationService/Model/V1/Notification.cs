using Microsoft.Extensions.Configuration.UserSecrets;

namespace NotificationService.Model.V1
{
    public class Notification
    {
        //   public DateTime Date { get; set; }
        public int UserID { get; set; }

        public string Message { get; set; }


    }
}