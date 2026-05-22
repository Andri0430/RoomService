using Application.Interfaces.IUser;
using Domain.Entities;
using Infrastructures.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _dbContext;

        public UserRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task Create(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users
                .AsNoTracking()
                .OrderByDescending(user => user.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);
        }

        public async Task HardDelete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDelete(User user)
        {
            user.IsDeleted = true;
            user.SetUpdate();
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
