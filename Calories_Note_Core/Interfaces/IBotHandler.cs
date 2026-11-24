namespace Calories_Note_Core.Interfaces;

public interface IBotHandler
{
    String Name { get; }

    Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token);
}
