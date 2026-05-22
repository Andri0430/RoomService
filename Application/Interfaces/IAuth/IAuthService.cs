using Application.Dtos.AuthDto;
using Application.Models;

namespace Application.Interfaces.IAuth
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
}
