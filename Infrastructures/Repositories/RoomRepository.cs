using Application.Interfaces.IRoom;
using Domain.Entities;
using Infrastructures.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDBContext _dbContext;

        public RoomRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task Create(Room room)
        {
            await _dbContext.Rooms.AddAsync(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .OrderBy(room => room.RoomName)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _dbContext.Rooms.FirstOrDefaultAsync(room => room.Id == id);
        }

        public async Task<Room?> GetByNameRoomAsync(string room)
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.RoomName.ToLower().Trim() == room.ToLower().Trim());
        }

        public async Task Delete(Room room)
        {
            _dbContext.Rooms.Remove(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Room room)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
