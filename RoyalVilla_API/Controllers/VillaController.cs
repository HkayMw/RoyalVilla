using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public VillaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<VillaDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<VillaDto>>>> GetVillas()
        {
            try
            {
                var villas = await _context.Villas.ToListAsync();
                var responseData = _mapper.Map<List<VillaDto>>(villas);

                var responseOk = ApiResponse<List<VillaDto>>.Ok("Villas retrieved successfully", responseData);

                return Ok(responseOk);

            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<List<object>>.Error(StatusCodes.Status500InternalServerError, "Error retrieving Villas from the database", e.Message);
                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<VillaDto>>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var responseEr = ApiResponse<object>.BadRequest("Invalid Villa ID. ID must be greater than zero.", null);

                    return BadRequest(responseEr);
                }
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {id} not found.");

                    return NotFound(responseEr);
                }

                var responseData = _mapper.Map<VillaDto>(villa);
                var responseOK = ApiResponse<VillaDto>.Ok("Villa retrieved successfully", responseData);

                return Ok(responseOK);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occured while retrieving a Villa with ID {id}", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> CreateVilla([FromBody] CreateVillaDto createVillaDto)
        {
            try
            {
                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createVillaDto.Name.ToLower());
                if (duplicateVilla != null)
                {
                    var responseEr = ApiResponse<object>.Conflict($"A Villa with Name: '{createVillaDto.Name}' already exists", null);
                    return Conflict(responseEr);
                }

                Villa villa = _mapper.Map<Villa>(createVillaDto);
                await _context.Villas.AddAsync(villa);
                await _context.SaveChangesAsync();

                var responseData = _mapper.Map<VillaDto>(villa);
                var responseOk = ApiResponse<VillaDto>.Created("New Villa created successfully", responseData);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while creating a Villa.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> UpdateVilla(int id, [FromBody] UpdateVillaDto updateVillaDto)
        {
            try
            {
                if (id != updateVillaDto.Id)
                {
                    var responseEr = ApiResponse<object>.BadRequest("Villa ID in url does not match Villa ID in request body.", null);

                    return BadRequest(responseEr);
                }


                var existingVilla = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
                if (existingVilla == null)
                {
                    var responseEr = ApiResponse<VillaDto>.NotFound($"Villa with ID {id} was not found.");

                    return NotFound(responseEr);
                }

                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == updateVillaDto.Name.ToLower() && v.Id != id);
                if (duplicateVilla != null)
                {
                    var responseEr = ApiResponse<object>.Conflict($"A Villa with Name: '{updateVillaDto.Name}' already exists", null);
                    return Conflict(responseEr);
                }

                _mapper.Map(updateVillaDto, existingVilla);

                existingVilla.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                var responseData = _mapper.Map<VillaDto>(updateVillaDto);

                var responseOk = ApiResponse<VillaDto>.Created("Villa updated successfully", responseData);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while updating a villa.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }


        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id < 0)
                {
                    var responseEr = ApiResponse<object>.BadRequest($"ID should be greater than 0.");

                    return BadRequest(responseEr);
                }

                var existingVilla = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

                if (existingVilla == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {id} was not found.");

                    return NotFound(responseEr);
                }

                _context.Villas.Remove(existingVilla);
                await _context.SaveChangesAsync();

                var responseOk = ApiResponse<object>.Ok($"Villa with ID {id} was deleted successfully.", null);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while deleting a villa.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }
    }
}
