using Calories_Note_Core.Interfaces;
using Telegram.Bot;

namespace Calories_Note_Infrastructure.Services;

public sealed class TelegramSender : IMessageSender
{
    private readonly ITelegramBotClient _botClient;

    public TelegramSender(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessageAsync(long chatId, String text, CancellationToken token)
    {
        await _botClient.SendMessage(chatId, text, cancellationToken: token);
    }
}