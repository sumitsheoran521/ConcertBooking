using Microsoft.AspNetCore.Mvc;

namespace ConcertBooking.Web.Controllers
{
    public class ConcertsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
