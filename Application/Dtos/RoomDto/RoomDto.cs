namespace Application.Dtos.RoomDto
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AddRoomDto
    {
        public string RoomName { get; set; } = string.Empty;
    }

    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
    }
}
