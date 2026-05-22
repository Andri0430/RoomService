using Application.Interfaces.IHistoryReservationRoom;
using Domain.Entities;
using Infrastructures.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class HistoryReservationRoomRepository : IHistoryReservationRoomRepository
    {
        private readonly AppDBContext _dbContext;

        public HistoryReservationRoomRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HistoryReservationRoom>> GetAllAsync()
        {
            return await _dbContext.HistoryReservationRooms
                .AsNoTracking()
                .Include(reservation => reservation.Room)
                .OrderByDescending(reservation => reservation.BookedDateReservation)
                .ToListAsync();
        }

        public async Task Create(HistoryReservationRoom reservation)
        {
            await _dbContext.HistoryReservationRooms.AddAsync(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsRoomBookedOnDateAsync(int roomId, DateTime bookedDate)
        {
            var date = bookedDate.Date;

            return await _dbContext.HistoryReservationRooms
                .AsNoTracking()
                .AnyAsync(reservation =>
                    reservation.RoomId == roomId &&
                    reservation.BookedDateReservation.Date == date);
        }

        public async Task<bool> HasRunningReservationByRoomIdAsync(int roomId)
        {
            var today = DateTime.Today;

            return await _dbContext.HistoryReservationRooms
                .AsNoTracking()
                .AnyAsync(reservation =>
                    reservation.RoomId == roomId &&
                    reservation.BookedDateReservation.Date >= today);
        }

        public async Task<List<HistoryReservationRoom>> GetHistoryReservationRoomByEmail(string email)
        {
            return await _dbContext.HistoryReservationRooms
                .AsNoTracking()
                .Where(reservation => reservation.UserEmail.ToLower().Trim() == email.ToLower().Trim())
                .Include(reservation => reservation.Room)
                .OrderByDescending(reservation => reservation.BookedDateReservation)
                .ToListAsync();
        }
    }
}
