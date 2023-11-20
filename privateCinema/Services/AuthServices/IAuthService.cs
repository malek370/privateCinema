using privateCinema.DTOs.LoginDTOs;

namespace privateCinema.Services.AuthServices
{
    public interface IAuthService
    {
        Task<Response<string>> Login(LoginDTO loguser);
        Task<Response<object>> Register(RegisterDTO reguser);
    }
}