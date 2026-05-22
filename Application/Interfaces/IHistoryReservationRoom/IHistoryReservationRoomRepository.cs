using Application.Dtos;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.IHistoryReservationRoom
{
    public interface IHistoryReservationRoomRepository
    {
        Task<List<HistoryReservationRoom>> GetAllAsync();
        Task Create(HistoryReservationRoom reservation);
        Task<bool> IsRoomBookedOnDateAsync(int roomId, DateTime bookedDate);
        Task<bool> HasRunningReservationByRoomIdAsync(int roomId);
        Task<List<HistoryReservationRoom>> GetHistoryReservationRoomByEmail(string email);
    }
}
