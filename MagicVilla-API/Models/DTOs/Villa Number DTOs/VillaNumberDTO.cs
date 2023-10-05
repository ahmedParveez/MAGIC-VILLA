using MagicVilla_API.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTOs.Villa_Number_DTOs
{
    public class VillaNumberDTO
    {
        [Required]
        public int villaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
		public VillaDTO Villa { get; set; } // This is a navigation property.

		// Here we are populating the villaDTO only when we have to get the complete details of the villa.

	}
}
