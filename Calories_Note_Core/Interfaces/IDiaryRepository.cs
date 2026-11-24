using Calories_Note_Core.Models;

namespace Calories_Note_Core.Interfaces;

public interface IDiaryRepository
{
    Task AddEntryAsync(DiaryEntry entry);
    Task<List<DiaryEntry>> GetEntriesByUserAndDateAsync(long userId, DateTime date);
    Task<double> GetTotalCaloriesByUserAndDateAsync(long userId, DateTime date);
}
