﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertBooking.Web.ViewModels.DashboardViewModels
{
    public class AvailableTicketsViewModel
    {
        public int ConcertId { get; set; }
        public string ConcertName { get; set; }
        public List<int> AvailableSeats { get; set; }
    }
}
