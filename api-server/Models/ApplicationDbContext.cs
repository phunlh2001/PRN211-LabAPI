using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api_server.Models
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
      // Database.EnsureCreated();
      // Database.Migrate();
    }

    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Category>().HasData(
          new Category()
          {
            ID = 1,
            Name = "Samsung",
          },
          new Category()
          {
            ID = 2,
            Name = "Apple",
          },
          new Category()
          {
            ID = 3,
            Name = "Nokia",
          }
      );
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
  }
}