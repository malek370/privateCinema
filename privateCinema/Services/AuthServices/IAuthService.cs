using Ath.DTOs.LoginDTOs;

namespace Ath.Services.AuthServices
{
    public interface IAuthService
    {
        Task<Response<string>> Login(LoginDTO loguser);
        Task<Response<object>> Register(RegisterDTO reguser);
    }
}