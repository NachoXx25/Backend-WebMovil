using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BlacklistedToken> BlacklistedToken {get; set;}
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}