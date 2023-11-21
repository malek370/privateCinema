using privateCinema.DTOs.UserDTOs;

namespace privateCinema.Services.UsersServices
{
    public interface IUserServices
    {
        Task<Response<object>> ChangeRole(AddRoleDTO userole);
        Task<Response<object>> DeleteUser(string email);
        Task<Response<List<GetUserDTO>>> GetAll();
        Task<Response<GetUserDTO>> GetByEmail(string email);
    }
}