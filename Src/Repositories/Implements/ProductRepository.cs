using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context; //Se crea un atributo de tipo DataContext

        public ProductRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product); //Se agrega el producto a la base de datos
            await _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
        }

        public async Task<IEnumerable<Product>> AdminSearchProducts(string search)
        {
            var products = await GetProducts(); // Obtener todos los productos disponibles
                    
            if (!string.IsNullOrEmpty(search)) // Si la búsqueda no es nula o vacía
            {
                products = products.Where(p =>
                    p.Id.ToString().Equals(search) ||
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Type.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Stock.ToString().Equals(search) ||
                    p.Price.ToString().Equals(search) ||
                    p.Image.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList(); // Filtrar los productos por id, nombre, tipo, stock, precio y imagen
            }
                    
            return products; // Retornar los productos filtrados
        }

        public async Task<IEnumerable<Product>> AvailableProducts()
        {
            var products = await _context.Products.Where(p => p.Stock > 0).ToListAsync(); //Se obtienen los productos disponibles
            return products; //Se retornan los productos
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            if(product == null) //Si el producto no existe
            {
                return false; //Se retorna falso
            }
            _context.Products.Remove(product); //Se elimina el producto
            await _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
            return true;  //Se retorna verdadero
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

        public async Task<Product?> GetProductByNameAndType(string name, string type)
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

        public async Task<IEnumerable<Product>> SearchProducts(string searchString)
        {
            var products = await AvailableProducts(); // Obtener todos los productos disponibles
            
            if (!string.IsNullOrEmpty(searchString)) // Si la búsqueda no es nula o vacía
            {
                products = products.Where(p =>
                    p.Id.ToString().Equals(searchString) ||
                    p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Type.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Stock.ToString().Equals(searchString) ||
                    p.Price.ToString().Equals(searchString) ||
                    p.Image.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList(); // Filtrar los productos por id, nombre, tipo, stock, precio y imagen
            }
            
            return products; // Retornar los productos filtrados
        }
    }
}