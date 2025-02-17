using Domain.Entities;

namespace LibraryWebApi.Services.UserService;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}
