using Application.Dtos;
using Application.Models;

namespace Application.Interfaces.IUser
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetAllAsync();
        Task<ApiResponse<UserDto?>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(AddUserDto dto);
        Task<ApiResponse<string>> UpdateAsync(UpdateUserDto dto);
        Task<ApiResponse<string>> HardDeleteAsync(int id);
        Task<ApiResponse<string>> SoftDeleteAsync(int id);
    }
}