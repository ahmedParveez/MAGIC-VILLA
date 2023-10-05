using AutoMapper;
using MagicVilla_MVC.Models;
using MagicVilla_MVC.Models.DTO;
using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using MagicVilla_MVC.Models.VM;
using MagicVilla_MVC.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace MagicVilla_MVC.Controllers
{
    /// <summary>
    ///   // Note:

    //#1. Add VM
    //#2. deserialize the response recieved from the 'GetAsync' to the villaNumberDTO that the user wants to update.
    //#3. Mapping from VillaNumberDTO to the respective DTO.
    //#4. Populate the Drop-down.
    //#5. Once Updated Return back to the View else Not Found.
    //#6. Add View Model (VM).
    //#7. Send the Request for POST,PUT or DELETE.
    //#8. If the operation is completed, redirect to the View
    //#9. If operation fails, redirect to the same page.
    //#10. Now if the model state is Invalid,we have to keep the villa list populated.
    /// </summary>
     

    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaService = villaService;
            _villaNumberService = villaNumberService;
            _mapper = mapper;
        }
        // Type ctrl+K,D to format the document

        // ===================================================================================================
        // ============================================== INDEX ==============================================
        // ===================================================================================================

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        // ==================================================================================================
        // ============================================== CREATE ============================================
        // ==================================================================================================

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM vm = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null)
            {
                vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).
                    Select(i => new SelectListItem // Here we are using the projection.
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); // Basically this will populate the drop-down, which will be IEnumerable of the select list item.
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else if (response.ErrorMessages.Count > 0)
                {
                    // Here we are displaying the error messages when something goes wrong.
                    // Also we can get complete list parse that and add multiple model errors.

                    ModelState.AddModelError("ErrorMessage", response.ErrorMessages.FirstOrDefault());
                }
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).
                    Select(i => new SelectListItem // Here we are using the projection.
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); // Basically this will populate the drop-down, which will be IEnumerable of the select list item.
            }

            TempData["error"] = "Error encountered.";
            return View(model);
        }

        // =================================================================================================
        // ============================================== UPDATE ===========================================
        // =================================================================================================

        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM vm = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null)
            {

                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                vm.VillaNumber = _mapper.Map<UpdateNumberDTO>(model);
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null)
            {
                vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).
                    Select(i => new SelectListItem // Here we are using the projection.
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); // Basically this will populate the drop-down, which will be IEnumerable of the select list item.
                return View(vm);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null)
                {
                    TempData["success"] = "Villa updated successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else if (response.ErrorMessages.Count > 0)
                {
                    // Here we are displaying the error messages when something goes wrong.
                    // Also we can get complete list parse that and add multiple model errors.
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessages.FirstOrDefault());
                }
            }

            // If anything is is not valid, We have to populate the dropdown again and redirect back.

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).
                    Select(i => new SelectListItem // Here we are using the projection.
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); // Basically this will populate the drop-down, which will be IEnumerable of the select list item.
            }

            TempData["error"] = "Error encountered.";
            return View(model);
        }

        // ====================================================================================================
        // ============================================== DELETE ==============================================
        // ====================================================================================================


        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM vm = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null)
            {

                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                vm.VillaNumber = model;
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null)
            {
                vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).
                    Select(i => new SelectListItem // Here we are using the projection.
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); // Basically this will populate the drop-down, which will be IEnumerable of the select list item.
                return View(vm);

            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.villaNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
