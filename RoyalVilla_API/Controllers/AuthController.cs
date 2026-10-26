using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                // validations
                if (registrationRequestDto is null)
                {
                    var responseEr = ApiResponse<object>.BadRequest($"User registration data is required.");
                    return StatusCode(responseEr.StatusCode, responseEr);
                }
                //check email exist
                var emailExists = await _authService.IsEmailExistsAsync(registrationRequestDto.Email);
                if (emailExists)
                {
                    //throw new InvalidOperationException($"The Email '{registrationRequestDto.Email}' already belongs to an existing acount");
                    var responseEr = ApiResponse<object>.Conflict($"The Email '{registrationRequestDto.Email}' already belongs to an existing acount.");
                    return StatusCode(responseEr.StatusCode, responseEr);
                }
                // call auth service
                var result = await _authService.RegisterAsync(registrationRequestDto);

                var responseOk = ApiResponse<UserDto>.Created("User created successfully", result);

                return CreatedAtAction(nameof(Register), responseOk);

            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "Error occured during user registration.", e.Message);
                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                if (loginRequestDto is null)
                {
                    var responseEr = ApiResponse<object>.BadRequest($"User login data is required.");
                    return StatusCode(responseEr.StatusCode, responseEr);
                }
                //check email exist
                var userExists = await _authService.IsEmailExistsAsync(loginRequestDto.Email);
                if (!userExists)
                {
                    var responseEr = ApiResponse<object>.NotFound($"User not found");
                    return StatusCode(responseEr.StatusCode, responseEr);
                }
                // call auth service
                var result = await _authService.LoginAsync(loginRequestDto);

                if (result is null)
                {
                    var responseEr = ApiResponse<object>.UnAuthorized("wrong password");

                    return StatusCode(responseEr.StatusCode, responseEr);
                }

                var responseOk = ApiResponse<LoginResponseDto>.Ok("User logged in successfully", result);

                return Ok(responseOk);

            }
            catch (Exception e)
            {
                var responseEr = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, "Error occured during login.", e.Message);
                return StatusCode(responseEr.StatusCode, responseEr);
            }
        }
    }
}
