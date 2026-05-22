using Application.Dtos;
using Application.Models;

namespace Application.Interfaces.IHistoryReservationRoom
{
    public interface IHistoryReservationRoomService
    {
        Task<ApiResponse<string>> CreateAsync(AddHistoryReservationRoomDto dto);
        Task<ApiResponse<List<HistoryReservationRoomDto>>> GetFinishedReservationsAsync();
        Task<ApiResponse<List<HistoryReservationRoomDto>>> GetRunningReservationsAsync();
        Task<ApiResponse<List<HistoryReservationRoomDto>>> GetHistoryReservationRoomByEmail(string email);

    }
}
