using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertBooking.Application.Common;
using ConcertBooking.Domain.Models;
using ConcertBooking.Infrastructure.Data;

namespace ConcertBooking.Infrastructure.Repository
{
    public class ConcertRepository : GenericRepository<Concert>, IConcertRepository
    {

        private readonly ApplicationDbContext _context;

        public ConcertRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void UpdateConcert(Concert concert)
        {
           _context.Concert.Update(concert);
        }
    }
}
