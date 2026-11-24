using Calories_Note_Core.Models;

namespace Calories_Note_Core.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByNameAsync(String name);
    Task<List<Product?>> SearchManyByNameAsync(String[] keywords);
}
