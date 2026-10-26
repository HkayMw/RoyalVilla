namespace RoyalVilla.DTO
{
    public class VillaAmenitiesDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int VillaId { get; set; }
        public string? VillaName { get; set; }
    }
}
