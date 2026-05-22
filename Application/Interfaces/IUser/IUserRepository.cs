using Domain.Entities;

namespace Application.Interfaces.IUser
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task Create(User user);
        Task Update(User user);
        Task SoftDelete(User user);
        Task HardDelete(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneNumber(string phoneNumber);
    }
}
