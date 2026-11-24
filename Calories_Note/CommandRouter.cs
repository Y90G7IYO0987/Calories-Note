using Calories_Note_Core.Interfaces;

namespace Calories_Note_Core;

public class CommandRouter
{
    private readonly Dictionary<string, IBotHandler> _commands;

    public CommandRouter(IEnumerable<IBotHandler> commands)
    {
        _commands = commands.ToDictionary(c => c.Name, c => c);
    }

    public async Task RouteAsync(long chatId, String message, IMessageSender sender, CancellationToken token)
    {
        if (string.IsNullOrEmpty(message))
            return;

        String[] msg = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        String command = msg[0];

        String[] args = msg.Skip(1).ToArray();

        if (_commands.TryGetValue(command, out IBotHandler? cmd))
            await cmd.ExecuteAsync(chatId, sender, args, token);
        else
            await sender.SendMessageAsync(chatId, "Данная команда не найдена!", token);
    }
}
