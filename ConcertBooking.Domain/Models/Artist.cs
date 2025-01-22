﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertBooking.Domain.Models
{

    // Artist(1)-------------------(*)Concert

    public class Artist
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Concert> Concerts { get; set; } = new List<Concert>();

    }
}
