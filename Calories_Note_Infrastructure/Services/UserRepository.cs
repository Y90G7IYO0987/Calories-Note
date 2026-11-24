using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;
using Calories_Note_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calories_Note_Infrastructure.Services;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByTelegramIdAsync(long telegramId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    public async Task AddOrUpdateAsync(User user)
    {
        var existingUser = await GetByTelegramIdAsync(user.TelegramId);
        if (existingUser == null)
            await _context.Users.AddAsync(user);
        else
        {
            existingUser.Name = user.Name;
            existingUser.Height = user.Height;
            existingUser.Weight = user.Weight;
            existingUser.Age = user.Age;
            existingUser.Gender = user.Gender;
            existingUser.Goal = user.Goal;
        }

        await _context.SaveChangesAsync();
    }
}
