using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;

namespace ContactsAPI.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterDto request);
        Task<UserManagerResponse> LoginUserAsync(LoginDto request);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }    
}
