using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Services
{
    public class AmenitiesService : IAmenitiesService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AmenitiesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<VillaAmenities> CreateVillaAmenities(CreateVillaAmenitiesDto createVillaAmenitiesDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteVillaAmenities(int id)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<VillaAmenities>> GetVillaAmenitiesAsync()
        {
            var amenities = await _context.VillaAmenities.ToListAsync();
            var results = _mapper.Map<List<VillaAmenities>>(amenities);
            return results;
        }

        public Task<VillaAmenities> GetVillaAmenitiesById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<VillaAmenities> UpdateVillaAmenities(UpdateVillaAmenitiesDto updateVillaAmenitiesDto)
        {
            throw new NotImplementedException();
        }
    }
}
