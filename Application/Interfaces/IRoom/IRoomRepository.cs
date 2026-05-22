using Domain.Entities;

namespace Application.Interfaces.IRoom
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task Create(Room room);
        Task Update(Room room);
        Task Delete(Room room);
        Task<Room?> GetByNameRoomAsync(string room);
    }
}
