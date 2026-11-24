using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;
using System.Text;

namespace Calories_Note.Handlers;

public class SearchCommand : IBotHandler
{
    public String Name => "/search";

    private readonly IProductRepository _productRepo;

    public SearchCommand(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }    

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {

        if (args.Length == 0)
        {
            await sender.SendMessageAsync(chatId, "Укажи продукт! Например: /search Яблоко", token);
            return;
        }

        String searchQuery = string.Join(" ", args);

        Product? product = await _productRepo.GetByNameAsync(searchQuery);

        if (product == null)
        {
            List<Product?> searchResult = await _productRepo.SearchManyByNameAsync(args);

            if (searchResult == null || searchResult.Count == 0)
            {
                await sender.SendMessageAsync(chatId, $"Продукт {searchQuery} не найден.", token);
                return;
            }

            await sender.SendMessageAsync(chatId, GenerateSimilarString(searchQuery, searchResult!), token);

            return;
        }

        String response = $"🍏 *{product.Name}*\n" +
                          $"Калории: {product.Calories} ккал\n" +
                          $"Белки: {product.Protein} г\n" +
                          $"Жиры: {product.Fat} г\n" +
                          $"Углеводы: {product.Carbs} г";

        await sender.SendMessageAsync(chatId, response, token);
    }

    private string GenerateSimilarString(string searchQuery, List<Product> foundProducts)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"🔍 По запросу *'{searchQuery}'* найдено похожее:\n\n");

        foreach (var product in foundProducts)
        {
            String calories = product.Calories.ToString("0.#");
            String protein = product.Protein.ToString("0.#");
            String fat = product.Fat.ToString("0.#");
            String carbs = product.Carbs.ToString("0.#");

            sb.Append($"▪️ *{product.Name}*\n");
            sb.Append($"├ Калории: {calories} ккал\n");
            sb.Append($"├ Белки: {protein} г\n");
            sb.Append($"├ Жиры: {fat} г\n");
            sb.Append($"└ Углеводы: {carbs} г\n\n");
        }

        sb.Append("Уточни запрос, чтобы получить БЖУ конкретного продукта.");

        return sb.ToString();
    }
}
