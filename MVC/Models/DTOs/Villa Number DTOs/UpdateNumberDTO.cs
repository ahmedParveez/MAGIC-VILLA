﻿using System.ComponentModel.DataAnnotations;

namespace MVC.Models.DTOs.Villa_Number_DTOs
{
    public class UpdateNumberDTO
    {
        [Required]
        public int villaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
    }
}
