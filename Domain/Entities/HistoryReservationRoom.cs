namespace Domain.Entities
{
    public class HistoryReservationRoom : BaseEntity
    {
        public int RoomId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTime BookedDateReservation { get; set; }
        public Room? Room { get; set; }

        public HistoryReservationRoom(int roomId, string userEmail, DateTime bookedDateReservation)
        {
            if (roomId <= 0) throw new ArgumentException("Ruangan tidak valid", nameof(roomId));
            if (string.IsNullOrWhiteSpace(userEmail)) throw new ArgumentException("Email pengguna tidak boleh kosong", nameof(userEmail));

            RoomId = roomId;
            UserEmail = userEmail.Trim();
            BookedDateReservation = bookedDateReservation;
        }
    }
}