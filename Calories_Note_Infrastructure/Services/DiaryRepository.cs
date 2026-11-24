using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;
using Calories_Note_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calories_Note_Infrastructure.Services;

public sealed class DiaryRepository : IDiaryRepository
{
    private readonly AppDbContext _context;

    public DiaryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddEntryAsync(DiaryEntry entry)
    {
        await _context.DiaryEntries.AddAsync(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<DiaryEntry>> GetEntriesByUserAndDateAsync(long userId, DateTime date)
    {
        DateTime targetDate = date.Date;

        return await _context.DiaryEntries
            .Include(d => d.Product)
            .Where(d => d.UserId == userId && d.Date.Date == targetDate)
            .ToListAsync();
    }

    public async Task<Double> GetTotalCaloriesByUserAndDateAsync(long userId, DateTime date)
    {
        return await _context.DiaryEntries
            .Where(d => d.UserId == userId && d.Date.Date == date.Date)
            .SumAsync(d => d.TotalCalories);
    }
}
