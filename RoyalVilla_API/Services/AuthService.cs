using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoyalVilla_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        private string GenerateJwtToken(User user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            try
            {
                // retrieve user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

                if (user is null) return null;

                // check password 
                if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, loginRequestDto.Password) == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                // map entity to response dto
                var UserDto = _mapper.Map<UserDto>(user);

                // generate jwt token
                var token = GenerateJwtToken(user);

                // return response dto
                return new LoginResponseDto
                {
                    UserDto = UserDto,
                    Token = token
                };


            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An unxpected error occured during user log in.", e);

            }
        }

        public async Task<UserDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            try
            {

                // map request dto to entity
                User user = _mapper.Map<User>(registrationRequestDto);
                user.Password = new PasswordHasher<User>().HashPassword(user, registrationRequestDto.Password);

                // add entity
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // map entity to response dto
                var response = _mapper.Map<UserDto>(user);

                // return response dto
                return response;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An unxpected error occured during user registration", e);
            }
        }
    }
}
