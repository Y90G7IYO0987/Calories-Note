namespace Calories_Note_Core.Interfaces;

public interface IMessageSender
{
    Task SendMessageAsync(long chatId, String text, CancellationToken token);
}