using Microsoft.EntityFrameworkCore;
using Calories_Note_Core.Models;

namespace Calories_Note_Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<DiaryEntry> DiaryEntries { get; set; }
    public DbSet<User> Users { get; set; }
}
