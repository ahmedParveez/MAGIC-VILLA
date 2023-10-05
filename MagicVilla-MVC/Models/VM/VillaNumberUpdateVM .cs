using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace MagicVilla_MVC.Models.VM
{
    
    public class VillaNumberUpdateVM
    {
        public UpdateNumberDTO VillaNumber { get; set; }
        public VillaNumberUpdateVM()
        {
            VillaNumber = new UpdateNumberDTO();
        }

        // For drop-down
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; } 
    }
}
