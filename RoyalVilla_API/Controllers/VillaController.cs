using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public string GetVillas()
        {
            return "Getting all villas";
        }

        [HttpGet("{id:int}")]
        public string GetVilla(int id)
        {
            return $"Getting villa with ID: {id}";
        }
    }
}
