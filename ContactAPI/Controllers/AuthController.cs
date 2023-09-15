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
        private IMailService _mailService;
        private IConfiguration _configuration;
        public AuthController(IUserService userService, 
            IMailService mailService, IConfiguration configuration)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }

        // /api/auth/register
        [HttpPost("regitser")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(request);
                if (result.IsSuccess)
                {                    
                    return Ok(result); //Status Code: 200
                }                    

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
                {
                    await _mailService.SendEmailAsync(request.Email, "New Login", "<h1>Hey!, new Login to your account noticed</h1><p>New Login to your account at : " + DateTime.Now + "</p>");
                    return Ok(result);
                }                    

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        // /api/auth/confirmEmail?userId&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["ApiUrl"]}/ConfirmEmail.html");
            }

            return BadRequest(result);
        }

        // /api/auth/ForgetPassword
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

            var result = await _userService.ForgetPasswordAsync(email);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // /api/auth/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(resetPasswordDto);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }         

            return BadRequest("Some properties are not valid");
        }
    }
}
