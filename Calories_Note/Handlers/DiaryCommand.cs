using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;
using System.Text;

namespace Calories_Note.Handlers;

public class DiaryCommand : IBotHandler
{
    public String Name => "/diary";

    private readonly IDiaryRepository _diaryRepository;

    public DiaryCommand(IDiaryRepository diaryRepository)
    {
        _diaryRepository = diaryRepository;
    }

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        List<DiaryEntry> entries = await _diaryRepository.GetEntriesByUserAndDateAsync(chatId, DateTime.UtcNow);

        if (entries.Count == 0)
        {
            await sender.SendMessageAsync(chatId, "Дневник пустой.", token);
            return;
        }

        Double totalCalories = entries.Sum(e => e.TotalCalories);

        StringBuilder sb = new StringBuilder();

        sb.Append("📅 Твой сегодняшний дневник:\n\n");

        foreach (var entry in entries)
        {
            String productName = entry.Product?.Name ?? "Неизвестный продукт";

            sb.Append($"▪️ *{productName}*\n");
            sb.Append($"├ Граммовка: {entry.WeightInGrams} г\n");
            sb.Append($"└ Калории: {entry.TotalCalories} ккал\n\n");
        }

        sb.Append($"🔥 Итого: *{totalCalories:F1}* ккал");

        await sender.SendMessageAsync(chatId, sb.ToString(), token);
    }
}
