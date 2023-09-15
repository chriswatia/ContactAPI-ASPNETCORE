using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactsAPI.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public UserService(UserManager<IdentityUser> userManager,
            IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
        }        

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterDto request)
        {
            if(request is null)
            {
                throw new NullReferenceException("Register request is null");
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false
                };
            }
                

            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);

            if (result.Succeeded)
            {
                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{_configuration["ApiUrl"]}/api/auth/confirmemail?userId={identityUser.Id}&token={validEmailToken}";

                await _mailService.SendEmailAsync(identityUser.Email, "Confirm your Email", $"<h1>Welcome to Contacts API</h1>" +
                    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");
                
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new UserManagerResponse
                {
                    Message = "User does not exist",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Password",
                    IsSuccess = false
                };
            }

            var claims = new[]
            {
                new Claim("Email", request.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            //Encrypt Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //Generate Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(3600),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "User Does not exist",
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Email not confirmed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user exists with the email"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            //generate Url
            string url = $"{_configuration["ApiUrl"]}/ResetPassword?email={email}&token={validToken}";

            //send Email
            await _mailService.SendEmailAsync(email, "Reset Password", $"<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click Here</a></p>");

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "Reset password link has been sent to your email!"
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user exists with the email"
                };
            }

            if(resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Passwords do not match!"
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, resetPasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Password has been reset successfully!"
                };
            }

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = " Something went wrong",
                 Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}
