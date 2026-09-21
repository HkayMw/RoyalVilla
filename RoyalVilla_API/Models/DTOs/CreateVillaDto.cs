namespace RoyalVilla_API.Models.DTOs
{
    public class CreateVillaDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}
