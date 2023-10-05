using AutoMapper;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using MagicVilla_API.Models.DTOs.Villa_Number_DTOs;
using MagicVilla_API.Models.Entities;
using MagicVilla_API.Repositories.Interfaces;
using MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumber")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        // ==========================================================================================================================
        protected APIResponse _response;
        private readonly IVillaNumberRepo _dbVillaNumber;
        private readonly IVillaRepo _dbVilla;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberRepo dbVillaNumber, IMapper mapper,
            IVillaRepo dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _response = new();
            _dbVilla = dbVilla;
        }


        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "string2" };
        }

        // Get all

        [HttpGet]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        // Get by villa Number

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.villaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Post

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] CreateNumberDTO createDTO)
        {
            try
            {

                if (await _dbVillaNumber.GetAsync(u => u.villaNo == createDTO.villaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _dbVilla.GetAsync(u => u.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);


                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.villaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Delete 

        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.villaNo == id);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        
        //Update

       // [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] UpdateNumberDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.villaNo)
                {
                    return BadRequest();
                }
                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                await _dbVillaNumber.UpdateNumberAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        //===========================================================================================================================


        //// Dependency injections 

        //protected APIResponse _response;
        //private readonly IVillaRepo _repo;
        //private readonly IVillaNumberRepo _numberRepo;
        //private readonly IMapper _mapper;
        //private readonly ILogging _logger;
        //public VillaNumberController(ILogging logger, IVillaRepo repo, IMapper mapper, IVillaNumberRepo numberRepo)
        //{
        //	this._response = new();
        //	_mapper = mapper;
        //	_logger = logger;
        //	_repo = repo;
        //	_numberRepo = numberRepo;
        //}

        //// Get All Villa's List

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<APIResponse>>> GetAll()
        //{
        //	try
        //	{
        //		_logger.Log("Success", "Getting all villas");
        //		IEnumerable<VillaNumber> villaList = await _numberRepo.GetAllAsync(includeProperties:"Villa");
        //		_response.Result = _mapper.Map<List<VillaNumberDTO>>(villaList);
        //		_response.isSuccess = true;
        //		_response.StatusCode = HttpStatusCode.OK;
        //		return Ok(_response);
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}

        //// Get Villa By "Id"

        //[HttpGet("{villaNumber:int}", Name = "GetVillaNumber")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNumber)
        //{
        //	try
        //	{
        //		if (villaNumber <= 0 || villaNumber == null)
        //		{
        //			_logger.Log("Error", $"Get villa error with Id {villaNumber}");
        //			return BadRequest();
        //		}
        //		var villa = await _repo.GetAsync(villaId => villaId.Id == villaNumber, includeProperties: "Villa");
        //		if (villa == null)
        //		{
        //			_logger.Log("Error", "Villa Not found");
        //			return NotFound();
        //		}
        //		_logger.Log("success", $"Getting villa with Id {villaNumber}");
        //		_response.Result = _mapper.Map<VillaDTO>(villa);
        //		_response.StatusCode = HttpStatusCode.OK;
        //		_response.isSuccess = true;
        //		return Ok(_response);
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}

        //// Create New Villa

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] CreateNumberDTO villaDTO)
        //{
        //	try
        //	{
        //		if (villaDTO == null)
        //		{
        //			_logger.Log("Error", "No input founded");
        //			return BadRequest(villaDTO);
        //		}
        //		if (await _numberRepo.GetAsync(number => number.villaNo == villaDTO.villaNo) != null)
        //		{
        //			_logger.Log("Error", "Ye wala villa to already list me hai. Koi naya villa ka naam soch!!!");
        //			ModelState.AddModelError("", "Ye wala villa to already list me hai. Koi naya villa ka naam soch!!!");
        //		}
        //		//villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        //		VillaNumber data = _mapper.Map<VillaNumber>(villaDTO);
        //		await _numberRepo.CreateAsync(data);
        //		_logger.Log("success", "Villa added successfully");
        //		_response.Result = _mapper.Map<VillaNumber>(villaDTO);
        //		_response.StatusCode = HttpStatusCode.Created;
        //		_response.isSuccess = true;
        //              //return CreatedAtRoute("GetVilla", new { villaNumber = data.villaNo}, _response); Here's an error
        //              return CreatedAtRoute("GetVillaNumber", new { villaNumber = data.villaNo }, _response);
        //          }
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}

        //// Delete Villa

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpDelete("{villaNumber:int}", Name = "DeleteVillaNumber")]
        //public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNumber)
        //{
        //	try
        //	{
        //		if (villaNumber == 0)
        //		{
        //			_logger.Log("error", $"Invalid villa Id {villaNumber}");
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			_response.isSuccess = false;
        //			return BadRequest();
        //		}
        //		var villa = await _numberRepo.GetAsync(u => u.villaNo == villaNumber);
        //		if (villa == null)
        //		{
        //			_logger.Log("Error", "Villa doesn't exist in the list");
        //			_response.StatusCode = HttpStatusCode.NotFound;
        //			_response.isSuccess = false;
        //			return NotFound();
        //		}
        //		await _numberRepo.RemoveAsync(villa);
        //		_logger.Log("Success", $"Villa Id {villaNumber} has been removed from the list successfully");
        //		_response.StatusCode = HttpStatusCode.NoContent;
        //		_response.isSuccess = true;
        //		return Ok(_response);
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}

        //// Update Complete villa

        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPut("{villaNumber:int}", Name = "UpdateVillaNumber")]
        //public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNumber, [FromBody] UpdateDTO villaDTO)
        //{
        //	try
        //	{
        //		//if (id == 0 || villaDTO == null)
        //		//{
        //		//    _logger.Log("error", $"Invalid input or villa Id {id}");
        //		//    return BadRequest();
        //		//}
        //		//var villa = await _repo.GetAsync(villaId => villaId.Id == id);
        //		//if (villa == null)
        //		//{
        //		//    return NotFound();
        //		//}
        //		//Villa model = _mapper.Map<Villa>(villaDTO);
        //		//await _repo.UpdateAsync(model);
        //		//_logger.Log("", "Villa updated successfully");
        //		//return NoContent();

        //		// ===================================================

        //		if (villaNumber == 0 || villaDTO == null)
        //		{
        //			_logger.Log("error", $"Invalid input or villa Id {villaDTO}");
        //			return BadRequest();
        //		}
        //		VillaNumber model = _mapper.Map<VillaNumber>(villaDTO);
        //		await _numberRepo.UpdateNumberAsync(model);
        //		await _numberRepo.SaveAsync();
        //		_response.Result = _mapper.Map<VillaNumber>(villaDTO);
        //		_response.isSuccess = true;
        //		_response.StatusCode = HttpStatusCode.OK;
        //		return Ok(_response);
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}
        //// Partially Update villa

        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPatch("{villaNumber:int}", Name = "UpdatePartialVillaNumber")]
        //public async Task<ActionResult<APIResponse>> UpdatePartialVillaNumber(int villaNumber, JsonPatchDocument<UpdateNumberDTO> patchDTO)
        //{
        //	try
        //	{

        //		//if (id == 0 || patchDTO == null)
        //		//{
        //		//    _logger.Log("error", $"Invalid input or villa Id {id}");
        //		//    return BadRequest();
        //		//}
        //		//var villa = await _repo.GetAsync(u => u.Id == id, isTracked: false);
        //		//if (villa == null)
        //		//{
        //		//    _logger.Log("error", $"Sorry the villa Id {id} doesn't exist in the list");
        //		//    return NotFound();
        //		//}
        //		//UpdateDTO villaDTO = _mapper.Map<UpdateDTO>(villa);
        //		//patchDTO.ApplyTo(villaDTO, ModelState);
        //		//if (!ModelState.IsValid)
        //		//{
        //		//    _logger.Log("error", $"Model state Error");
        //		//    return BadRequest(ModelState);
        //		//}
        //		//_logger.Log("Success", "Villa Updated successfully");
        //		//Villa model = _mapper.Map<Villa>(villaDTO);
        //		//await _repo.UpdateAsync(model);
        //		//await _repo.SaveAsync();
        //		//return NoContent();

        //		// =================================================

        //		if (patchDTO == null || villaNumber == null || villaNumber <= 0)
        //		{
        //			_logger.Log("error", $"Invalid input or villa Id {villaNumber}");
        //			return BadRequest();
        //		}
        //		var villa = await _numberRepo.GetAsync(villaId => villaId.villaNo == villaNumber, isTracked: false);
        //		if (villa == null)
        //		{
        //			_logger.Log("error", $"Sorry the villa Id {villaNumber} doesn't exist in the list");
        //			return NotFound();
        //		}
        //		UpdateNumberDTO villaDTO = _mapper.Map<UpdateNumberDTO>(villa);

        //		// Now let's patch
        //		patchDTO.ApplyTo(villaDTO, ModelState);

        //		// Mapp to villa
        //		VillaNumber model = _mapper.Map<VillaNumber>(villaDTO);
        //		await _numberRepo.UpdateNumberAsync(model);
        //		_response.Result = _mapper.Map<VillaNumber>(villaDTO);
        //		_response.isSuccess = true;
        //		_response.StatusCode = HttpStatusCode.OK;
        //		return NoContent();
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.isSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string> { ex.Message };
        //		return BadRequest(_response);
        //	}
        //}
    }
}
