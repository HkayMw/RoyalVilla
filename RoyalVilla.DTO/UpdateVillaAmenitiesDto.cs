using System.ComponentModel.DataAnnotations;

namespace RoyalVilla.DTO
{
    public class UpdateVillaAmenitiesDto
    {
        //[Required]
        //public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        //[Required]
        //public int VillaId { get; set; }

    }
}
