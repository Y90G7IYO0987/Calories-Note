
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Calories_Note_Infrastructure.Data;

public class FactoryDbContext : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=CaloriesBot;Username=postgres;Password=0000");

        return new AppDbContext(optionsBuilder.Options);
    }
}
