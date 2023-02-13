﻿using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Routes
    {
        public int Id { get; set; }

        [Required]
        public string? Region { get; set; }

        [Required]
        public string? Town { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public ICollection<Stops> Stops { get; set; }

        public ICollection<UserDetails> DriverDetails { get; set; }
    }   
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
