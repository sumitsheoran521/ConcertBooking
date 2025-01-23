﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertBooking.Application.Common;
using ConcertBooking.Domain.Models;
using ConcertBooking.Infrastructure.Data;

namespace ConcertBooking.Infrastructure.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
