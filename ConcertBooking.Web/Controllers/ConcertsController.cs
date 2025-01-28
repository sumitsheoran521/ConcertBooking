using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Domain.Models;
using ConcertBooking.Web.ViewModels.ConcertViewModels;
using ConcertBooking.Web.ViewModels.DashboardViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConcertBooking.Web.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly IArtistService _artistService;
        private readonly IBookingService _bookingService;
        private readonly IConcertService _concertService;
        private readonly IUtilityService _utilityService;
        private readonly IVenueService _venueService;
        private string containerName = "ConcertImage";

        public ConcertsController(IArtistService artistService, IBookingService bookingService, IConcertService concertService, IUtilityService utilityService, IVenueService venueService, string containerName)
        {
            _artistService = artistService;
            _bookingService = bookingService;
            _concertService = concertService;
            _utilityService = utilityService;
            _venueService = venueService;
            this.containerName = containerName;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var concerts = _concertService.GetAllConcert();
            List<ConcertViewModel> list = new List<ConcertViewModel>();
            foreach (var item in concerts)
            {
                list.Add(new ConcertViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    DateTime = item.DateTime,
                    ArtistName = item.Artist.Name,
                    VenueName = item.Venue.Name,
                });
            };
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var venues = _venueService.GetAllVenue();
            var artists = _artistService.GetAllArtist();
            ViewBag.VenueList = new SelectList(venues, "Id", "Name");
            ViewBag.ArtistList = new SelectList(artists, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateConcertViewModel vm)
        {
            var concert = new Concert
            {
                Name = vm.Name,
                Description = vm.Description,
                DateTime = vm.DateTime,
                ArtistId = vm.ArtistId,
                VenueId = vm.VenueId,
            };
            if (vm.ImageUrl != null)
            {
                concert.ImageUrl = await _utilityService.SaveImage(containerName, vm.ImageUrl);
            }
            await _concertService.SaveConcert(concert);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var concert = _concertService.GetConcert(id);
            var artists = _artistService.GetAllArtist();
            var venues = _venueService.GetAllVenue();
            ViewBag.artistList = new SelectList(artists, "Id", "Name");
            ViewBag.venueList = new SelectList(venues, "Id", "Name");
            var vm = new EditConcertViewModel
            {
                Id = concert.Id,
                Name = concert.Name,
                ImageUrl = concert.ImageUrl,
                DateTime = concert.DateTime,
                ArtistId = concert.ArtistId,
                VenueId = concert.VenueId,
                Description = concert.Description
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditConcertViewModel vm)
        {
            var concert = _concertService.GetConcert(vm.Id);
            concert.Id = vm.Id;
            concert.Name = vm.Name;
            concert.Description = vm.Description;
            concert.DateTime = vm.DateTime;
            concert.ArtistId = vm.ArtistId;
            concert.VenueId = vm.VenueId;
            if (vm.ChooseImage != null)
            {
                concert.ImageUrl = await _utilityService.EditImage(containerName, vm.ChooseImage, concert.ImageUrl);
            }
            _concertService.UpdateConcert(concert);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var concert = _concertService.GetConcert(id);
            if (concert != null)
            {
                await _utilityService.DeleteImage(containerName, concert.ImageUrl);
                await _concertService.DeleteConcert(concert);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetTickets(int id)
        {
            var booking = _bookingService.GetAllBooking(id);
            var vm = booking.Select(b => new DashboardViewModel
            {
                UserName = b.ApplicationUser.UserName,
                ConcertName = b.Concert.Name,
                SeatNumber = string.Join(", ", b.Tickets.Select(t => t.SeatNumber))
            }).ToList();
            return View(vm);

        }
    }
}
