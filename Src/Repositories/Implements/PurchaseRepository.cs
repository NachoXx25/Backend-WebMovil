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

        public async Task<IEnumerable<Purchase>> ByUserId(int id)
        {
            var purchases = await _context.Purchases
                                        .Where(p => p.UserId == id)
                                        .ToListAsync(); //Se obtienen las compras por el id del usuario
            return purchases; //Se retornan las compras
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
    }
}