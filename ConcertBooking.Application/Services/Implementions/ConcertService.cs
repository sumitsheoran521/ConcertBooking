﻿using System;
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
    public class ConcertService : IConcertService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConcertService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteConcert(Concert concert)
        {
            ArgumentNullException.ThrowIfNull(concert);
            await _unitOfWork.ConcertRepository.DeleteAsync(concert);
            _unitOfWork.Save();
        }

        public IEnumerable<Concert> GetAllConcert()
        {
            return _unitOfWork.ConcertRepository.GetAll(include: x => x.Include(a => a.Artist).Include(v => v.Venue));
        }

        public Concert GetConcert(int id)
        {
            var concert = _unitOfWork.ConcertRepository.GetByIdAsync(filter: x => x.Id == id, include: z => z.Include(a => a.Artist).Include(v => v.Venue));
            if (concert == null)
            {
                throw new Exception("Concert null");
            }
            return concert;
        }

        public async Task SaveConcert(Concert concert)
        {
            ArgumentNullException.ThrowIfNull(concert);
            await _unitOfWork.ConcertRepository.AddAsync(concert);
            _unitOfWork.Save();
        }

        public void UpdateConcert(Concert concert)
        {
            ArgumentNullException.ThrowIfNull(concert);
            _unitOfWork.ConcertRepository.UpdateConcert(concert);
            _unitOfWork.Save();
        }
    }
}
