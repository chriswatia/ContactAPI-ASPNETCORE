using ContactsAPI.Dtos;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // /api/auth/register
        [HttpPost("regitser")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(request);
                if (result.IsSuccess)
                    return Ok(result); //Status Code: 200

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status Code: 400
        }

        // /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(request);
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }
    }
}
