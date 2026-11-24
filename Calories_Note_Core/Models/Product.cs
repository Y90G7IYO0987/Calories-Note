namespace Calories_Note_Core.Models;

public class Product
{
    public Int32 Id { get; set; }
    public String Name { get; set; } = string.Empty;
    public Double Calories { get; set; }
    public Double Protein { get; set; }
    public Double Fat { get; set; }
    public Double Carbs { get; set; }
}