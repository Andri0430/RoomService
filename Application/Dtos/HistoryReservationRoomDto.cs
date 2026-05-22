namespace Application.Dtos
{
    public class HistoryReservationRoomDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime BookedDateReservation { get; set; }
    }

    public class AddHistoryReservationRoomDto
    {
        public int RoomId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTime BookedDateReservation { get; set; }
    }
}
