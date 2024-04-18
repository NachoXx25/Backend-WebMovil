using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context; //Inyección de dependencia

        public ProductRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product); //Se agrega el producto a la base de datos
            await _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
        }


        public async Task<bool> DeleteProduct(Product product)
        {
            if(product == null)
            {
                return false;
            }
            _context.Products.Remove(product); //Se elimina el producto
            await _context.SaveChangesAsync();
            return true;  
        }


        public Task<Product?> GetProductById(int id)
        {
            var product = _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync(); //Se obtiene el producto por su id
            if(product == null)
            {
                throw new Exception("Product not found");
            }
            return product; //Se retorna el producto encontrado
            
        }

        public async Task<Product> GetProductByNameAndType(string name, string type)
        {
            var product = await _context.Products.Where(p => p.Name == name & p.Type == type).FirstOrDefaultAsync(); //Se obtiene el producto por su nombre y tipo
            if(product != null)
            {
                throw new Exception("Producto ya existe");
            }
            return product; //Se retorna el producto encontrado
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.ToListAsync(); //Se obtienen todos los productos
            return products;
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
        }
    }
}