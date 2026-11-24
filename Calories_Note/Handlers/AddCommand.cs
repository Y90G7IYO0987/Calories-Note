using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;

namespace Calories_Note.Handlers;

public class AddCommand : IBotHandler
{
    public String Name => "/add";

    private readonly IDiaryRepository _diaryRepository;
    private readonly IProductRepository _productRepository;

    public AddCommand(IDiaryRepository diaryRepository, IProductRepository productRepository)
    {
        _diaryRepository = diaryRepository;
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        if (args.Length < 2)
        {
            await sender.SendMessageAsync(chatId, "Формат: /add [продукт] [граммы]. Пример: /add яблоко 150", token);
            return;
        }

        String weight = args[^1];

        if (!Double.TryParse(weight, out Double grams) || grams <= 0)
        {
            await sender.SendMessageAsync(chatId, $"Вес должен быть положительным числом! Например: 150", token);
            return;
        }

        String productName = String.Join("", args[..^1]);

        Product? product = await _productRepository.GetByNameAsync(productName);

        if (product == null)
        {
            await sender.SendMessageAsync(chatId, $"Продукт {productName} не найден. Попробуй ввести команду /search [Имя Продукта]", token);
            return;
        }

        Double totalCalories = (product.Calories / 100.0) * grams;

        DiaryEntry entry = new DiaryEntry()
        {
            UserId = chatId,
            ProductId = product.Id,
            WeightInGrams = grams,
            TotalCalories = totalCalories,
            Date = DateTime.UtcNow
        };

        await _diaryRepository.AddEntryAsync(entry);

        String response = $"✅ Добавлено: *{product.Name}* ({grams} г)\n" +
                          $"Калории: {totalCalories:F1} ккал";

        await sender.SendMessageAsync(chatId, response, token);
    }
}
