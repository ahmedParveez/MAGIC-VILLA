using MagicVilla_MVC.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs
{
    public class VillaNumberDTO
    {
        [Required]
        public int villaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
        public VillaDTO Villa { get; set; }
    }
}
