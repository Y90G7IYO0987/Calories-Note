using Calories_Note_Core.Interfaces;

namespace Calories_Note.Handlers;

public class HelpCommand : IBotHandler
{
    public String Name => "/help";

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        await sender.SendMessageAsync(chatId, "🤖 Доступные команды:\r\n\r\n/start — Начать работу\r\n/help — Показать это сообщение\r\n/search [название] — Найти продукт и узнать его БЖУ\r\n/diary — Открыть дневник питания (что съел)\r\n/add [продукт] [граммы] — Добавить еду в дневник\r\n/norm — Рассчитать твою норму калорий\r\n/stats — Статистика питания за сегодня\r\n\r\n💡 Пример: /add яблоко 150", token);
    }
}
