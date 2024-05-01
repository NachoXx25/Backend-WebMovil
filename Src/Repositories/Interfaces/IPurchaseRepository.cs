using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<bool> GetProductPurchaseById(int id);

        Task<IEnumerable<Purchase>> GetPurchases();

        Task MakePurchase(Purchase purchase);
    }
}