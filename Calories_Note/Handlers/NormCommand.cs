using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;

namespace Calories_Note.Handlers;

public sealed class NormCommand : IBotHandler
{
    public String Name => "/norm";

    private readonly IUserRepository _userRepository;

    public NormCommand(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(long chatId, IMessageSender sender, String[] args, CancellationToken token)
    {
        if (args.Length == 0)
        {
            User? existingUser = await _userRepository.GetByTelegramIdAsync(chatId);

            if (existingUser == null)
            {
                await sender.SendMessageAsync(chatId,
                    "Ты еще не вводил данные.\nНапиши команду: /norm [вес] [рост] [возраст] [пол M/F] [цель].\n" +
                    "цели:\n├ 0 -> Похудеть\n├ 2 -> Набрать массу\n└ 1 -> Поддерживать форму", token);
                return;
            }

            Double existingBmr = existingUser.Gender == "M"
                ? 10 * existingUser.Weight + 6.25 * existingUser.Height - 5 * existingUser.Age + 5
                : 10 * existingUser.Weight + 6.25 * existingUser.Height - 5 * existingUser.Age - 161;

            Double existingTdee = existingBmr * 1.375;

            Double existingNorm = existingUser.Goal switch
            {
                0 => existingTdee - 500,
                2 => existingTdee + 500,
                _ => existingTdee
            };

            await sender.SendMessageAsync(chatId,
                $"📊 Твоя норма: *{existingNorm:F0} ккал/день*\n" + 
                $"Вес: {existingUser.Weight} кг, Рост: {existingUser.Height} см, Возраст: {existingUser.Age}\n" +
                $"Цель: {(existingUser.Goal == 0 ? "Похудеть" : existingUser.Goal == 2 ? "Набрать" : "Поддерживать")}",
                token);

            return;
        }

        if (args.Length < 5)
        {
            await sender.SendMessageAsync(chatId,
                "Формат: /norm [вес] [рост] [возраст] [пол M/F] [цель].\n" +
                "цели:\n├ 0 -> Похудеть\n├ 2 -> Набрать массу\n└ 1 -> Поддерживать форму", token);
            return;
        }

        if (!float.TryParse(args[0], out float weight) ||
            !float.TryParse(args[1], out float height) ||
            !int.TryParse(args[2], out int age) ||
            !int.TryParse(args[4], out int goal))
        {
            await sender.SendMessageAsync(chatId , "Ошибка в данных. Проверь формат.", token);
            return;
        }

        String gender = args[3].ToUpper();

        Double bmr;

        if (gender == "M")
            bmr = 10 * weight + 6.25 * height - 5 * age + 5;
        else
            bmr = 10 * weight + 6.25 * height - 5 * age - 161;

        Double tdee = bmr * 1.375;

        Double norm = goal switch
        {
            0 => tdee - 500,
            2 => tdee + 500,
            _ => tdee
        };
        
        User user = new User()
        {
            TelegramId = chatId,
            Weight = weight,
            Height = height,
            Age = age,
            Gender = gender,
            Goal = goal
        };

        await _userRepository.AddOrUpdateAsync(user);

        await sender.SendMessageAsync(chatId,
            $"Твоя норма калорий: *{norm:F0} ккал/день*\n" +
            $"(Базовый метаболизм: {bmr:F0} ккал)", token);
    }
}
