using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Web.ViewModels;
using ConcertBooking.Web.ViewModels.DashboardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConcertBooking.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public IActionResult MyTickets()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;

            var bookings = _ticketService.GetBookings(userid);
            List<BookingViewModel> vm = new List<BookingViewModel>();
            foreach (var booking in bookings)
            {
                vm.Add(new BookingViewModel
                {
                    BookingId = booking.Id,
                    BookingDate = booking.BookingDate,
                    ConcertName = booking.Concert.Name,
                    Tickets = booking.Tickets.Select(t => new TicketViewModel
                    {
                        SeatNumber = t.SeatNumber
                    }).ToList(),
                });
            }

            return View(vm);
        }
    }
}
