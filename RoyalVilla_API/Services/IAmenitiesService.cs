using RoyalVilla.DTO;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Services
{
    public interface IAmenitiesService
    {
        Task<VillaAmenities> CreateVillaAmenities(CreateVillaAmenitiesDto createVillaAmenitiesDto);
        Task<List<VillaAmenities>> GetVillaAmenitiesAsync();
        Task<VillaAmenities> GetVillaAmenitiesById(int id);
        Task<VillaAmenities> UpdateVillaAmenities(UpdateVillaAmenitiesDto updateVillaAmenitiesDto);
        Task<bool> DeleteVillaAmenities(int id);
    }
}
