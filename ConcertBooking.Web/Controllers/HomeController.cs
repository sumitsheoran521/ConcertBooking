using System.Diagnostics;
using System.Security.Claims;
using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Domain.Models;
using ConcertBooking.Web.Models;
using ConcertBooking.Web.ViewModels.DashboardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConcertService _concertService;
        private readonly ITicketService _ticketService;
        private readonly IBookingService _bookingService;

        public HomeController(ILogger<HomeController> logger, IConcertService concertService, ITicketService ticketService, IBookingService bookingService)
        {
            _logger = logger;
            _concertService = concertService;
            _ticketService = ticketService;
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            var concerts = _concertService.GetAllConcert();
            var vm = concerts.Where(d => d.DateTime.Date >= today).Select(x => new HomeConcertViewModel
            {
                ConcertId = x.Id,
                ConcertName = x.Name,
                ArtistName = x.Artist.Name,
                ConcertImage = x.ImageUrl,
                Description = x.Description.Length > 100 ? x.Description.Substring(0, 100) + "..." : x.Description
            }).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var concert = _concertService.GetConcert(id);
            if (concert == null)
            {
                return NotFound();
            }
            var vm = new HomeConcertDetailsViewModel
            {
                ConcertId = concert.Id,
                ConcertName = concert.Name,
                ConcertDateTime = concert.DateTime,
                ArtistName = concert.Artist.Name,
                Description = concert.Description,
                ArtistImage = concert.Artist.ImageUrl,
                VenueName = concert.Venue.Name,
                VenueAddress = concert.Venue.Address,
                ConcertImage = concert.ImageUrl
            };
            return View(vm);
        }

        [Authorize]
        public IActionResult AvailableTickets(int id)
        {
            var concert = _concertService.GetConcert(id);
            if (concert == null)
            {
                return NotFound();
            }
            var allSeats = Enumerable.Range(1, concert.Venue.SeatCapacity).ToList();
            var bookedTickets = _ticketService.GetBookedTickets(concert.Id);
            var availableSeats = allSeats.Except(bookedTickets).ToList();

            var vm = new AvailableTicketsViewModel
            {
                ConcertId = concert.Id,
                ConcertName = concert.Name,
                AvailableSeats = availableSeats,
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult BookTickets(int ConcertId, List<int> selectedSeats)
        {
            if (selectedSeats == null || selectedSeats.Count == 0)
            {
                ModelState.AddModelError("", "No Seats Selected");
                return RedirectToAction("Available Tickets", new { id = ConcertId });
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
            var booking = new Booking()
            {
                ConcertId = ConcertId,
                BookingDate = DateTime.Now,
                ApplicationUserId = userid,
            };
            foreach (var seatnumber in selectedSeats)
            {
                booking.Tickets.Add(new Ticket
                {
                    SeatNumber = seatnumber,
                    IsBooked = true
                });
            }
            _bookingService.AddBooking(booking);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
