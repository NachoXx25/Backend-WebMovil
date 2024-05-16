using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository; //Inyección de dependencia
        private readonly IProductService _productService; //Inyección de dependencia

        private readonly IUserRepository _userRepository; //Inyección de dependencia

        private readonly IMapperService _mapperService; //Inyección de dependencia
        public PurchaseService(IPurchaseRepository purchaseRepository, IProductService productService, IUserRepository userRepository, IMapperService mapperService)
        {
            _purchaseRepository = purchaseRepository;
            _productService = productService;
            _userRepository = userRepository;
            _mapperService = mapperService;
        }
        public async Task GetProductPurchaseById(int id)
        {
            var confirm = await _purchaseRepository.GetProductPurchaseById(id); //Se obtiene la compra por su id
            if(confirm != false){ //Si la compra no existe
                throw new Exception("No se puede eliminar un producto que ya ha sido comprado."); // esta validacion deberia ir sabiendo que hay que desplegar el id de un producto en la venta pero si el producto no existe?
            }
            return;
        }

        public Task<Purchase> MakePurchase(int id, int quantity, int userId)
        {
            var product = _productService.GetProductById(id).Result; //Se obtiene el producto por su id
            var user = _userRepository.GetUserById(userId).Result; //Se obtiene el usuario por su id
            if(user == null){
                throw new Exception("Usuario no encontrado"); //Si el usuario no existe, se lanza una excepción
            }
            if(product == null){
                throw new Exception("Producto no encontrado"); //Si el producto no existe, se lanza una excepción
            }
            if(quantity > product.Stock){
                throw new Exception("No hay suficiente stock"); //Si la cantidad es mayor al stock, se lanza una excepción
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
            }; //Se crea un objeto de tipo Purchase
            _purchaseRepository.MakePurchase(purchase); //Se realiza la compra
            return Task.FromResult(purchase); //Se retorna la compra
        }

        public async Task<IEnumerable<PurchaseDTO>> SearchPurchase(string searchString)
        {
            var purchase = await _purchaseRepository.GetPurchases(); //Se obtienen todas las compras

            if (!string.IsNullOrEmpty(searchString)) //Si la cadena de búsqueda no está vacía
            {
                
                purchase = purchase.Where(p =>
                    p.ProductName.Contains(searchString, System.StringComparison.OrdinalIgnoreCase) ||
                    p.ProductType.Contains(searchString, System.StringComparison.OrdinalIgnoreCase) ||
                    p.UserId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                    p.Date.ToString("yyyy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                ); //Se filtran las compras por nombre, tipo, id de usuario y fecha
            }

            return purchase.Select(p => _mapperService.PurchaseToPurchaseDTO(p));  //Se mapean las compras a DTO
        }

        public async Task<IEnumerable<PurchaseDTO>> SearchTicket(string searchString)
        {
            
            var purchase = await _purchaseRepository.GetPurchases(); //Se obtienen todas las compras

            if (!string.IsNullOrEmpty(searchString))
            {
                purchase = purchase.Where(p => 
                    p.UserId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) // Convertir UserId a cadena para buscar
                ); //Se filtran las compras por id de usuario
            }

            purchase = purchase.OrderByDescending(p => p.Date); //Se ordenan las compras por fecha

            return purchase.Select(p => _mapperService.PurchaseToPurchaseDTO(p));  //Se mapean las compras a DTO
        }


    }
}