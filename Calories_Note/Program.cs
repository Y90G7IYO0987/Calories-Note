using Calories_Note.Configuration;
using Calories_Note.Handlers;
using Calories_Note_Core;
using Calories_Note_Core.Interfaces;
using Calories_Note_Infrastructure.Data;
using Calories_Note_Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CaloriesBot;

public class Program
{
    private static TelegramSender _sender = null!;
    private static CommandRouter _router = null!;

    public static async Task Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var settings = new AppSettings();

        config.Bind(settings);

        if (string.IsNullOrEmpty(settings.Token))
        {
            Console.WriteLine("Ошибка: Токен не найден!");
            return;
        }

        using var cts = new CancellationTokenSource();

        var botClient = new TelegramBotClient(settings.Token);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;DataBase=CaloriesBot;Username=postgres;Password=0000");

        var context = new AppDbContext(optionsBuilder.Options);

        var productRepository = new ProductRepository(context);
        var diaryRepository = new DiaryRepository(context);
        var userRepository = new UserRepository(context);

        await DbSeeder.SeedAsync(context);

        List<IBotHandler> commands = new List<IBotHandler>()
        {
            new StartCommand(),
            new HelpCommand(),
            new SearchCommand(productRepository),
            new AddCommand(diaryRepository, productRepository),
            new DiaryCommand(diaryRepository),
            new NormCommand(userRepository),
            new StatsCommand(diaryRepository, userRepository)
        };

        _sender = new TelegramSender(botClient);
        _router = new CommandRouter(commands);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message },
            DropPendingUpdates = true
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions
        );

        var botInfo = await botClient.GetMe(cts.Token);

        Log.Information($"Бот @{botInfo.Username} успешно запущен и слушает чат!");

        Log.Information("Нажми Enter для завершения программы.");
        Console.ReadLine();

        Log.CloseAndFlush();

        await cts.CancelAsync();
    }

    /// <summary>
    /// Обработка сообщений от пользователей.
    /// </summary>
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        if (update.Message is not { Text: { } messageText } message)
            return;

        long chatId = message.Chat.Id;
        String? username = message.Chat.FirstName;

        Log.Information($"{username} пишет боту: {messageText}.");

        await _router.RouteAsync(chatId, messageText, _sender, token);
    }

    /// <summary>
    /// Обработка ошибок.
    /// </summary>
    private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception error, CancellationToken token)
    {
        Log.Error($"Ошибка: {error}!");

        await Task.CompletedTask;
    }
}