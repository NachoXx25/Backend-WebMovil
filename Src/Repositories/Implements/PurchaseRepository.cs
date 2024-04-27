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

    }
}