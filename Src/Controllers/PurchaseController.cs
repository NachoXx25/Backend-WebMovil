using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapperService _mapper;

        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IProductService productService, IMapperService mapper, IPurchaseService purchaseService)
        {
            _productService = productService;
            _mapper = mapper;
            _purchaseService = purchaseService;
        }
        [Authorize(Roles = "User")]
        [HttpGet ("all")]
        public ActionResult<ProductDTO> GetAvailableProducts(){
            var products = _productService.GetAvailableProducts().Result;
            var productDTOs = products.Select(p => _mapper.ProductToProductDTO(p)).ToList();
            if(productDTOs == null){
                return NotFound("No se encontraron productos");
            }
            return Ok(productDTOs);
        }
        [Authorize(Roles = "User")]
        [HttpPost("{id}/{quantity}/{userId}/buy")]
        public ActionResult<PurchaseDTO> MakePurchase(int id, int quantity, int userId){

            try{
                if(id <= 0){
                    return BadRequest("El id debe ser mayor a 0");
                }
                var product = _productService.GetProductById(id).Result;
                if(product == null){
                    return NotFound("Producto no encontrado");
                }
                if(quantity <= 0){
                    return BadRequest("La cantidad debe ser mayor a 0");
                }
                if(quantity > product.Stock){
                    return BadRequest("No hay suficiente stock");
                }
                var purchase = _purchaseService.MakePurchase(id, quantity, userId).Result;
                return Ok(_mapper.PurchaseToPurchaseDTO(purchase));
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("sales")]
        public ActionResult<PurchaseDTO> GetPurchases(){
            var purchase = _purchaseService.GetPurchases().Result;
            var purchaseDTOs = purchase.Select(p => _mapper.PurchaseToPurchaseDTO(p)).ToList();
            if(purchaseDTOs == null){
                return NotFound("No se encontraron compras");
            }
            return Ok(purchaseDTOs);
        }
    }
}