using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext _context;

        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> GetProductPurchaseById(int id)
        {
            var Product = await _context.Purchases.FirstOrDefaultAsync(p => p.Id == id);
            if(Product != null){
                return true;
            }
            return false;
        }

        public Task<IEnumerable<Purchase>> GetPurchases()
        {
            var purchases = _context.Purchases.ToList();
            return Task.FromResult<IEnumerable<Purchase>>(purchases);
        }

        public Task MakePurchase(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            var product = _context.Products.FirstOrDefault(p => p.Id == purchase.ProductId);
            if(product == null){
                throw new Exception("Producto no encontrado");
            }
            product.Stock -= purchase.Quantity;
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}