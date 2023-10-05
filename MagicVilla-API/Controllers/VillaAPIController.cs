using AutoMapper;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using MagicVilla_API.Models.Entities;
using MagicVilla_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/VillaAPI")]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogging _logger;
        public VillaAPIController(ILogging logger, IVillaRepo repo,IMapper mapper)
        {
            this._response = new();
            _mapper = mapper;
            _logger = logger;
            _repo = repo;
        }

        // Get All Villa's List

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<APIResponse>>> GetVillas()
        {
            try
            {
                _logger.Log("Success", "Getting all villas");
                IEnumerable<Villa> villaList = await _repo.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.isSuccess=false;
                _response.StatusCode=HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        // Get Villa By "Id"

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id <= 0 || id == null)
                {
                    _logger.Log("Error", $"Get villa error with Id {id}");
                    return BadRequest();
                }
                var villa = await _repo.GetAsync(villaId => villaId.Id == id);
                if (villa == null)
                {
                    _logger.Log("Error", "Villa Not found");
                    return NotFound();
                }
                _logger.Log("success", $"Getting villa with Id {id}");
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        // Create New Villa

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] CreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    _logger.Log("ErrorMessages", "No input founded");
                    return BadRequest(villaDTO);
                }
                if (await _repo.GetAsync(name => name.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    _logger.Log("ErrorMessages", "Ye wala villa to already list me hai. Koi naya villa ka naam soch!!!");
                    ModelState.AddModelError("", "Ye wala villa to already list me hai. Koi naya villa ka naam soch!!!");
                }
                //villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
                Villa data = _mapper.Map<Villa>(villaDTO);
                await _repo.CreateAsync(data);
                _logger.Log("success", "Villa added successfully");
                _response.Result = _mapper.Map<Villa>(villaDTO);
                _response.StatusCode = HttpStatusCode.Created;
                _response.isSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = data.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        // Delete Villa

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("error", $"Invalid villa Id {id}");
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    return BadRequest();
                }
                var villa = await _repo.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _logger.Log("Error", "Villa doesn't exist in the list");
                    _response.StatusCode=HttpStatusCode.NotFound;
                    _response.isSuccess=false;
                    return NotFound();
                }
                await _repo.RemoveAsync(villa);
                _logger.Log("Success", $"Villa Id {id} has been removed from the list successfully");
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        // Update Complete villa

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] UpdateDTO villaDTO)
        {
            try
            {
                //if (id == 0 || villaDTO == null)
                //{
                //    _logger.Log("error", $"Invalid input or villa Id {id}");
                //    return BadRequest();
                //}
                //var villa = await _repo.GetAsync(villaId => villaId.Id == id);
                //if (villa == null)
                //{
                //    return NotFound();
                //}
                //Villa model = _mapper.Map<Villa>(villaDTO);
                //await _repo.UpdateAsync(model);
                //_logger.Log("", "Villa updated successfully");
                //return NoContent();

                // ===================================================

                if (id == 0 || villaDTO == null)
                {
                    _logger.Log("error", $"Invalid input or villa Id {id}");
                    return BadRequest();
                }
                Villa model = _mapper.Map<Villa>(villaDTO);
                await _repo.UpdateAsync(model);
                await _repo.SaveAsync();
                _response.Result = _mapper.Map<Villa>(villaDTO);
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        // Partially Update villa

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<UpdateDTO> patchDTO)
        {
            try
            {

                //if (id == 0 || patchDTO == null)
                //{
                //    _logger.Log("error", $"Invalid input or villa Id {id}");
                //    return BadRequest();
                //}
                //var villa = await _repo.GetAsync(u => u.Id == id, isTracked: false);
                //if (villa == null)
                //{
                //    _logger.Log("error", $"Sorry the villa Id {id} doesn't exist in the list");
                //    return NotFound();
                //}
                //UpdateDTO villaDTO = _mapper.Map<UpdateDTO>(villa);
                //patchDTO.ApplyTo(villaDTO, ModelState);
                //if (!ModelState.IsValid)
                //{
                //    _logger.Log("error", $"Model state Error");
                //    return BadRequest(ModelState);
                //}
                //_logger.Log("Success", "Villa Updated successfully");
                //Villa model = _mapper.Map<Villa>(villaDTO);
                //await _repo.UpdateAsync(model);
                //await _repo.SaveAsync();
                //return NoContent();

                // =================================================

                if (patchDTO == null || id == null || id <= 0)
                {
                    _logger.Log("error", $"Invalid input or villa Id {id}");
                    return BadRequest();
                }
                var villa = await _repo.GetAsync(villaId => villaId.Id == id,isTracked: false);
                if (villa == null)
                {
                    _logger.Log("error", $"Sorry the villa Id {id} doesn't exist in the list");
                    return NotFound();
                }
                UpdateDTO villaDTO = _mapper.Map<UpdateDTO>(villa);

                // Now let's patch
                patchDTO.ApplyTo(villaDTO, ModelState);

                // Mapp to villa
                Villa model = _mapper.Map<Villa>(villaDTO);
                await _repo.UpdateAsync(model);
                _response.Result = _mapper.Map<Villa>(villaDTO);
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
    }
}

// Important Code

// #1. villaDTO.Id = VillaStore.villaList.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;
// #2. 