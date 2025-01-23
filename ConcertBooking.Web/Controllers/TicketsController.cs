using Microsoft.AspNetCore.Mvc;

namespace ConcertBooking.Web.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
