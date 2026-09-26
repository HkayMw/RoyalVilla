namespace RoyalVilla_API.Models.DTOs
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public UserDto? UserDto { get; set; }
    }
}
