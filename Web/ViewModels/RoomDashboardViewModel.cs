using Application.Dtos;
using Application.Dtos.RoomDto;

namespace Web.ViewModels
{
    public class RoomDashboardViewModel
    {
        public List<RoomDto> Rooms { get; set; } = new();
        public List<HistoryReservationRoomDto> FinishedReservations { get; set; } = new();
        public List<HistoryReservationRoomDto> RunningReservations { get; set; } = new();
    }
}
