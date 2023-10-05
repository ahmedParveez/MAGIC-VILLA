using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace MagicVilla_MVC.Models.VM
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDTO VillaNumber { get; set; }
        public VillaNumberDeleteVM()
        {
            VillaNumber = new VillaNumberDTO();
        }

        // For drop-down
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; } 
    }
}
