using LWS.Entities;
using Microsoft.EntityFrameworkCore;

namespace LWS.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<User> AccountUsers { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
    }
}
