using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa/{villaId:int}/amenities")]
    [ApiController]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VillaAmenitiesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDto>>> CreateVillaAmenities(int villaId, [FromBody] CreateVillaAmenitiesDto createVillaAmenitiesDto)
        {
            try
            {
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == villaId);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {villaId} not found.");

                    return NotFound(responseEr);
                }

                var duplicateVillaAmenity = await _context.VillaAmenities.FirstOrDefaultAsync(v => (v.Name.ToLower() == createVillaAmenitiesDto.Name.ToLower()) && (v.VillaId == villaId));
                if (duplicateVillaAmenity != null)
                {
                    var responseEr = ApiResponse<object>.Conflict($"A Villa Amenity with Name: '{createVillaAmenitiesDto.Name}' already exists", null);
                    return Conflict(responseEr);
                }

                var amenity = _mapper.Map<VillaAmenities>(createVillaAmenitiesDto);
                amenity.VillaId = villaId;
                await _context.VillaAmenities.AddAsync(amenity);
                await _context.SaveChangesAsync();

                var responseData = _mapper.Map<VillaAmenitiesDto>(amenity);
                var responseOk = ApiResponse<VillaAmenitiesDto>.Created("New Villa amenity created successfully", responseData);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while creating a Villa amenity.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVillaAmenities(int villaId, int id)
        {
            try
            {
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == villaId);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {villaId} not found.");

                    return NotFound(responseEr);
                }

                if (id <= 0)
                {
                    var responseEr = ApiResponse<object>.BadRequest($"ID should be greater than 0.");

                    return BadRequest(responseEr);
                }

                var existingVillaAmenity = await _context.VillaAmenities.FirstOrDefaultAsync(v => (v.Id == id) && (v.VillaId == villaId));

                if (existingVillaAmenity == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa Amenity with ID {id} and belongs to Villa with ID {villaId} was not found.");

                    return NotFound(responseEr);
                }

                _context.VillaAmenities.Remove(existingVillaAmenity);
                await _context.SaveChangesAsync();

                var responseOk = ApiResponse<object>.Ok($"Villa Amenity with ID {id} was deleted successfully.", null);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while deleting a villa amenity.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<VillaAmenitiesDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<VillaAmenitiesDto>>>> GetVillaAmenitiesAsync(int villaId)
        {
            try
            {
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == villaId);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {villaId} not found.");

                    return NotFound(responseEr);
                }
                var amenities = await _context.VillaAmenities
                    .Where(a => a.VillaId == villaId)
                    .Include(a => a.Villa)
                    .AsNoTracking()
                    .ToListAsync();
                var results = _mapper.Map<List<VillaAmenitiesDto>>(amenities);
                var responseOk = ApiResponse<List<VillaAmenitiesDto>>.Ok($"Villa Amenities for villa with ID {villaId} retreaved successfully", results);
                return Ok(responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<List<object>>.Error(StatusCodes.Status500InternalServerError, $"Error retrieving Amenities for villa with ID {villaId} from the database", e.Message);
                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDto>>> GetVillaAmenitiesByIdAsync(int villaId, int id)
        {
            try
            {
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == villaId);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {villaId} not found.");

                    return NotFound(responseEr);
                }
                if (id <= 0)
                {
                    var responseEr = ApiResponse<object>.BadRequest("Invalid ID. ID must be greater than zero.", null);

                    return BadRequest(responseEr);
                }
                var villaAmenity = await _context.VillaAmenities.FirstOrDefaultAsync(v => (v.Id == id) && (v.VillaId == villaId));
                if (villaAmenity == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa Amenity with ID {id} and belongs to Villa with ID {villaId} was not found.");

                    return NotFound(responseEr);
                }

                var responseData = _mapper.Map<VillaAmenitiesDto>(villaAmenity);
                var responseOK = ApiResponse<VillaAmenitiesDto>.Ok("Villa amenity retrieved successfully", responseData);

                return Ok(responseOK);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occured while retrieving a Villa amenity with ID {id}", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDto>>> UpdateVillaAmenities(int villaId, int id, UpdateVillaAmenitiesDto updateVillaAmenitiesDto)
        {
            try
            {
                var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == villaId);
                if (villa == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa with ID {villaId} not found.");

                    return NotFound(responseEr);
                }

                var existingVillaAmenity = await _context.VillaAmenities.FirstOrDefaultAsync(v => (v.Id == id) && (v.VillaId == villaId));
                if (existingVillaAmenity == null)
                {
                    var responseEr = ApiResponse<object>.NotFound($"Villa Amenity with ID {id} and belongs to Villa with ID {villaId} was not found.");
                    return NotFound(responseEr);
                }

                var duplicateVillaAmenity = await _context.VillaAmenities
                    .FirstOrDefaultAsync(v => v.Name.ToLower() == updateVillaAmenitiesDto.Name.ToLower()
                            && v.VillaId == villaId
                            && v.Id != id);
                if (duplicateVillaAmenity != null)
                {
                    var responseEr = ApiResponse<object>.Conflict($"A Villa amenity with Name: '{updateVillaAmenitiesDto.Name}' already exists", null);
                    return Conflict(responseEr);
                }

                _mapper.Map(updateVillaAmenitiesDto, existingVillaAmenity);

                existingVillaAmenity.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                var responseData = _mapper.Map<VillaAmenitiesDto>(existingVillaAmenity);

                var responseOk = ApiResponse<VillaAmenitiesDto>.Ok("Villa amenity updated successfully", responseData);

                return StatusCode(responseOk.StatusCode, responseOk);
            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "An error occured while updating a villa amenity.", e.Message);

                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }
    }
}
