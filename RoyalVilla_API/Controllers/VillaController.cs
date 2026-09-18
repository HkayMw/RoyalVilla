using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            try
            {
                return Ok(await _db.Villas.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Villas from the database: " + ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Villa>> GetVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Villa ID. ID must be greater than zero.");
                }
                var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Villa with ID {id}: " + ex.Message);
            }
        }
    }
}
