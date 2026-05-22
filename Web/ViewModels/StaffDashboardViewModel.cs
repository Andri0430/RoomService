using Application.Dtos;
using Application.Dtos.RoomDto;

namespace Web.ViewModels
{
    public class StaffDashboardViewModel
    {
        public List<RoomDto> Rooms { get; set; } = new();
        public List<HistoryReservationRoomDto> HistoryReservationRooms { get; set; } = new();
    }
}
