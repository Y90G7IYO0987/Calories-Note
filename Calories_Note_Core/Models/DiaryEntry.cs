namespace Calories_Note_Core.Models;

public class DiaryEntry
{
    public Int32 Id { get; set; }

    public long UserId { get; set; }
    public Int32 ProductId { get; set; }
    public Product? Product { get; set; }

    public Double WeightInGrams { get; set; }
    public Double TotalCalories { get; set; }
    public DateTime Date { get; set; }
}
