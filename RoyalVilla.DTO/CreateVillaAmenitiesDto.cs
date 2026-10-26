

using System.ComponentModel.DataAnnotations;

namespace RoyalVilla.DTO
{
    public class CreateVillaAmenitiesDto
    {
        [Required, MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
