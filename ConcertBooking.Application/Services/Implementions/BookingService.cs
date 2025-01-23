using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertBooking.Application.Common;
using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertBooking.Application.Services.Implementions
{
    public class BookingService : IBookingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddBooking(Booking booking)
        {
           ArgumentNullException.ThrowIfNull(booking);
            await _unitOfWork.BookingRepository.AddAsync(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBooking(int concertId)
        {
            var booking = _unitOfWork.BookingRepository.GetAll(filter: x => x.ConcertId == concertId, include: b => b.Include(a => a.Tickets).Include(c => c.Concert).Include(u => u.ApplicationUser));
            return booking;

        }
    }
}
