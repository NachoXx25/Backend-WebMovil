using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<bool> GetProductPurchaseById(int id); //Método para obtener una compra por su id

        Task<IEnumerable<Purchase>> GetPurchases(); //Método para obtener todas las compras

        Task MakePurchase(Purchase purchase); //Método para realizar una compra

        Task<IEnumerable<Purchase>> SearchPurchase(string search); //Obtener todas las compras filtrandolas por caracteristicas especificas

        Task<IEnumerable<Purchase>> SearchTicket(string searchString); //Obtener todos los tickets filtrandolos por el id del usuario
    }
}