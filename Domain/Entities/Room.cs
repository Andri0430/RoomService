namespace Domain.Entities
{
    public class Room : BaseEntity
    {
        public string RoomName { get; set; } = string.Empty;

        public Room(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Nama Ruangan tidak boleh kosong", nameof(roomName));

            RoomName = roomName.Trim();
        }

        public void Update(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Nama Ruangan tidak boleh kosong", nameof(roomName));

            RoomName = roomName.Trim();
            SetUpdate();
        }
    }
}
