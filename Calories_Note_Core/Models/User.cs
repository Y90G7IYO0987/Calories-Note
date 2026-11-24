namespace Calories_Note_Core.Models;

public class User
{
    public Int32 Id { get; set; }
    public long TelegramId { get; set; }
    public String? Name { get; set; }
    public Single Height { get; set; }
    public Single Weight { get; set; }
    public Int32 Age { get; set; }
    public String? Gender { get; set; }

    /// <summary>
    /// Цель. 0 - похудеть; 1 - держать; 2 - набрать
    /// </summary>
    public Int32 Goal { get; set; }
}
