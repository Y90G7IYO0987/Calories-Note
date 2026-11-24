using Calories_Note_Core.Interfaces;

namespace Calories_Note.Handlers;

public sealed class StartCommand : IBotHandler
{
    public String Name => "/start";

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        await sender.SendMessageAsync(chatId, "Привет! Я твой личный счетчик калорий 🤖\r\n\r\nВот что я умею:\r\n🍎 Искать продукты и смотреть их БЖУ\r\n📊 Вести дневник питания\r\n📅 Считать твою норму калорий\r\n📈 Показывать статистику за день\r\n\r\nНапиши /help, чтобы увидеть все команды.", token);
    }
}