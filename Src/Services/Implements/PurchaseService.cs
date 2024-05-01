using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductService _productService;
        public PurchaseService(IPurchaseRepository purchaseRepository, IProductService productService)
        {
            _purchaseRepository = purchaseRepository;
            _productService = productService;
        }
        
        public async Task GetProductPurchaseById(int id)
        {
            var confirm = await _purchaseRepository.GetProductPurchaseById(id);
            if(confirm != false){
                throw new Exception("No se puede eliminar un producto que ya ha sido comprado."); // esta validacion deberia ir sabiendo que hay que desplegar el id de un producto en la venta pero si el producto no existe?
            }
            return;
        }

        public Task<IEnumerable<Purchase>> GetPurchases()
        {
            var purchase = _purchaseRepository.GetPurchases().Result;
            return Task.FromResult<IEnumerable<Purchase>>(purchase);
        }

        public Task<Purchase> MakePurchase(int id, int quantity, int userId)
        {
            var product = _productService.GetProductById(id).Result;
            if(product == null){
                throw new Exception("Producto no encontrado");
            }
            if(quantity > product.Stock){
                throw new Exception("No hay suficiente stock");
            }
            var purchase = new Purchase{
                ProductId = product.Id,
                ProductName = product.Name,
                ProductType = product.Type,
                UnitPrice = product.Price,
                Quantity = quantity,
                UserId = userId,
                Total = product.Price * quantity,
                Date = DateTime.Now
            };
            _purchaseRepository.MakePurchase(purchase);
            return Task.FromResult(purchase);
        }
    }
}