using Application.Dtos;
using Application.Dtos.RoomDto;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.IRoom
{
    public interface IRoomsService
    {
        Task<ApiResponse<List<RoomDto>>> GetAllAsync();
        Task<ApiResponse<RoomDto?>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(AddRoomDto dto);
        Task<ApiResponse<string>> UpdateAsync(UpdateRoomDto dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<string>> GetByNameRoomAsync(string room);

    }
}
