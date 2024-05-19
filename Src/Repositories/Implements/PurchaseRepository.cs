using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext _context; //Se crea un atributo de tipo DataContext

        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> GetProductPurchaseById(int id)
        {
            var Product = await _context.Purchases.FirstOrDefaultAsync(p => p.Id == id); //Se obtiene la compra por su id
            if(Product != null){
                return true; //Si la compra existe, se retorna verdadero
            }
            return false; //Si la compra no existe, se retorna falso
        }

        public Task<IEnumerable<Purchase>> GetPurchases()
        {
            var purchases = _context.Purchases.ToList(); //Se obtienen todas las compras
            return Task.FromResult<IEnumerable<Purchase>>(purchases); //Se retornan las compras
        }

        public Task MakePurchase(Purchase purchase)
        {
            _context.Purchases.Add(purchase); //Se agrega la compra a la base de datos
            var product = _context.Products.FirstOrDefault(p => p.Id == purchase.ProductId); //Se obtiene el producto por su id
            if(product == null){
                throw new Exception("Producto no encontrado"); //Si el producto no existe, se lanza una excepción
            }
            product.Stock -= purchase.Quantity; //Se resta la cantidad de productos comprados al stock
            _context.SaveChanges(); //Se guardan los cambios en la base de datos
            return Task.CompletedTask; //Se retorna una tarea completada
        }

        public async Task<IEnumerable<Purchase>> SearchPurchase(string searchString)
        {
            var purchases = await _context.Purchases.ToListAsync(); // Obtener todas las compras

            if (!string.IsNullOrEmpty(searchString)) // Si la cadena de búsqueda no está vacía
            {
                if (DateTime.TryParse(searchString, out DateTime searchDate))
                {
                    purchases = purchases.Where(p =>
                        p.Date.Date == searchDate.Date // Comparar solo la parte de la fecha, ignorando la hora
                    ).ToList(); // Filtrar los usuarios por fecha de nacimiento
                }
                else{
                    purchases = purchases.Where(p =>
                        p.Id.ToString().Equals(searchString) ||
                        p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.ProductType.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.UserId.ToString().Equals(searchString) ||
                        p.Quantity.ToString().Equals(searchString) ||
                        p.Total.ToString().Equals(searchString) ||
                        p.UnitPrice.ToString().Equals(searchString) ||
                        p.ProductId.ToString().Equals(searchString) ||
                        p.Date.Day.ToString().Contains(searchString) ||
                        p.Date.Month.ToString().Contains(searchString) ||
                        p.Date.Year.ToString().Contains(searchString)
                    ).ToList(); // Filtrar las compras por Id, Producto nombre, Producto Tipo, user Id, cantidad, Total, precio unitario, Producto Id y fecha
                }
            }

            return purchases; // Retornar las compras filtradas
        }

        public async Task<IEnumerable<Purchase>> SearchTicket(string searchString)
        {
            var purchases = await _context.Purchases.ToListAsync(); // Obtener todas las compras

            if (!string.IsNullOrEmpty(searchString))
            {
                purchases = purchases.Where(p => 
                    p.UserId.ToString().Equals(searchString)
                ).ToList(); // Filtrar las compras por id de usuario
            }

            purchases = purchases.OrderByDescending(p => p.Date).ToList(); // Ordenar las compras por fecha

            return purchases; // Retornar las compras filtradas y ordenadas
        }
    }
}