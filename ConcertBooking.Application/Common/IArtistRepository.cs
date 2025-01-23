﻿using ConcertBooking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertBooking.Application.Common
{
    public interface IArtistRepository : IGenericRepository<Artist>
    {
        void UpdateArtist(Artist artist);
    }
}
