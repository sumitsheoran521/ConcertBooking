﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertBooking.Application.Common;
using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Domain.Models;

namespace ConcertBooking.Application.Services.Implementions
{
    public class VenueService : IVenueService
    {

        private readonly IUnitOfWork _unitOfWork;

        public VenueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteVenue(Venue venue)
        {
            ArgumentNullException.ThrowIfNull(venue);
            await _unitOfWork.VenueRepository.DeleteAsync(venue);
            _unitOfWork.Save();
        }

        public IEnumerable<Venue> GetAllVenue()
        {
            return _unitOfWork.VenueRepository.GetAll();
            
        }

        public Venue GetVenue(int id)
        {
            var venue = _unitOfWork.VenueRepository.GetById(id);
            if (venue == null)
            {
                throw new Exception("Venue null");
            }
            return venue;
        }

        public async Task SaveVenue(Venue venue)
        {
            ArgumentNullException.ThrowIfNull(venue);
            await _unitOfWork.VenueRepository.AddAsync(venue);
            _unitOfWork.Save();
        }

        public void UpdateVenue(Venue venue)
        {
            ArgumentNullException.ThrowIfNull(venue);
            _unitOfWork.VenueRepository.UpdateVenue(venue);
            _unitOfWork.Save();
        }
    }
}
