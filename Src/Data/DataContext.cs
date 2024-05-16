using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; } //se crea un DbSet de usuarios
        public DbSet<Role> Roles { get; set; } //se crea un DbSet de roles
        public DbSet<Product> Products { get; set; } //se crea un DbSet de productos

        public DbSet<Purchase> Purchases { get; set; } //se crea un DbSet de compras
        public DataContext(DbContextOptions options) : base(options) //se crea el constructor de la clase
        {
            
        }
    }
}