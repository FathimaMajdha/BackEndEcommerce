using BackendProject.Dto;
using BackendProject.ApiResponse;

namespace BackendProject.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Login(UserLoginDto dto);
        Task<ApiResponse<string>> Register(UserRegisterDto dto);
        Task<ApiResponse<UserResponseDto>> GetCurrentUser();
    }
}
