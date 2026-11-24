using Calories_Note_Core.Models;

namespace Calories_Note_Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByTelegramIdAsync(long telegramId);
    Task AddOrUpdateAsync(User user);
}
