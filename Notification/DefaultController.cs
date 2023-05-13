using Microsoft.AspNetCore.Mvc;

namespace Notification
{
    public class Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
