﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertBooking.Domain.Models;

namespace ConcertBooking.Application.Services.Interfaces
{
    public interface IArtistService
    {
        IEnumerable<Artist> GetAllArtist();
        Artist GetArtist(int id);
        Task SaveArtist(Artist artist);
        Task DeleteArtist(Artist artist);
        void UpdateArtist(Artist artist);
    }
}
