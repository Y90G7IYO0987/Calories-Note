using Calories_Note_Core.Interfaces;
using Calories_Note_Core.Models;
using Calories_Note_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calories_Note_Infrastructure.Services;

public sealed class ProductRepository : IProductRepository
{
    private AppDbContext _context = null!;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower().Contains(name.ToLower()));
    }

    public async Task<List<Product?>> SearchManyByNameAsync(String[] keywords)
    {
        if (keywords == null || keywords.Length == 0)
            return new List<Product?>();

        List<String> lowerKeywords = keywords.Select(k => k.ToLower()).ToList();

        List<Product> allProducts = await _context.Products.ToListAsync();

        List<Product> foundProducts = allProducts
            .Where(p => lowerKeywords.Any(k => p.Name.ToLower().Contains(k)))
            .Take(10)
            .ToList();

        return foundProducts!;
    }
}
