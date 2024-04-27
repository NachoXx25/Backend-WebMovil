using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }
        public async Task GetProductPurchaseById(int id)
        {
            var confirm = await _purchaseRepository.GetProductPurchaseById(id);
            if(confirm != false){
                throw new Exception("No se puede eliminar un producto que ya ha sido comprado."); // esta validacion deberia ir sabiendo que hay que desplegar el id de un producto en la venta pero si el producto no existe?
            }
            return;
        }

    }
}