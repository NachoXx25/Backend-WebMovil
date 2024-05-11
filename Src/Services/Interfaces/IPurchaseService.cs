using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IPurchaseService
    {
      Task GetProductPurchaseById(int id);   

      Task<Purchase> MakePurchase(int id, int quantity, int userId);

      Task<IEnumerable<PurchaseDTO>> SearchPurchase(string searchString);

      Task<IEnumerable<PurchaseDTO>> SearchTicket(string searchString);
    }
}