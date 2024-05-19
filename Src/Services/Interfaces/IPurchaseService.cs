using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IPurchaseService
    {
      Task<Purchase> MakePurchase(int id, int quantity, int userId); //Se realiza una compra

      Task<IEnumerable<PurchaseDTO>> SearchPurchase(string searchString); //Se buscan compras

      Task<IEnumerable<PurchaseDTO>> SearchTicket(string searchString); //Se buscan tickets
    }
}