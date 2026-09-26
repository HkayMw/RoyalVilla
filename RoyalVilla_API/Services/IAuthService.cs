using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTOs;

namespace RoyalVilla_API.Services
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> IsEmailExistsAsync(string email);
        //Task<string?> GenerateJwtToken(User user);
    }
}
