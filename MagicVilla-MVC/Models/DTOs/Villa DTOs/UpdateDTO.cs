﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_MVC.Models.DTO
{
    public class UpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }
    }
}
