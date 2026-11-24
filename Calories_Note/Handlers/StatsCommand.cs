using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;

namespace Calories_Note.Handlers;

public sealed class StatsCommand : IBotHandler
{
    public String Name => "/stats";

    private readonly IDiaryRepository _diaryRepository;
    private readonly IUserRepository _userRepository;

    public StatsCommand(IDiaryRepository diaryRepository, IUserRepository userRepository)
    {
        _diaryRepository = diaryRepository;
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        Double total = await _diaryRepository.GetTotalCaloriesByUserAndDateAsync(chatId, DateTime.UtcNow);

        User? user = await _userRepository.GetByTelegramIdAsync(chatId);

        String statusText = "Норма не установлена. Используй команду /norm";

        if (user != null)
        {
            Double dmr = user.Gender == "M"
                ? 10 * user.Weight + 6.25 * user.Height - 5 * user.Age + 5
                : 10 * user.Weight + 6.25 * user.Height - 5 * user.Age - 161;

            Double tdee = dmr * 1.375;

            Double norm = user.Goal switch
            {
                0 => tdee - 500,
                2 => tdee + 500,
                _ => tdee
            };

            Double remaining = norm - total;

            statusText = $"Норма: {norm:F0} ккал\n";
            statusText = remaining > 0
                ? $"Осталось: {remaining:F0} ккал"
                : $"⚠️ Перебор: {Math.Abs(remaining):F0} ккал";
        }

        await sender.SendMessageAsync(chatId,
            "📊 Статистика за сегодня:\n" +
            $"Съедено: {total:F0} ккал\n" +
            $"{statusText}", token);
    }
}
