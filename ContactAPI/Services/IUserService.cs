using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;

namespace ContactsAPI.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterDto request);
        Task<UserManagerResponse> LoginUserAsync(LoginDto request);
    }    
}
