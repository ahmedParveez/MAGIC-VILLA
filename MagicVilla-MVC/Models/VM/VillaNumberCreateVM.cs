using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace MagicVilla_MVC.Models.VM
{
    /// <summary>
    /// Here we are creating an View Model(VM) for our villa number.
    /// Also to create a new drop-down
    /// </summary>
    public class VillaNumberCreateVM
    {
        public CreateNumberDTO VillaNumber { get; set; }
        public VillaNumberCreateVM()
        {
            VillaNumber = new CreateNumberDTO();
        }

        // For drop-down
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; } 
    }
}
