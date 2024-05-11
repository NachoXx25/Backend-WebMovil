using System.IdentityModel.Tokens.Jwt;
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
        [HttpGet("searchProducts/{searchString}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string searchString)
        {
            try
            {
                var products = await _productService.SearchProducts(searchString);
                if (!products.Any())
                {
                    return NotFound("No se encontraron productos que coincidan con la búsqueda.");
                }
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost("{id}/{quantity}/buy")]
        public ActionResult<PurchaseDTO> MakePurchase(int id, int quantity){

            try{
                var token = Request.Headers["Authorization"].ToString().Split(' ')[1]; // Se obtiene el token
                var handler = new JwtSecurityTokenHandler(); // Se crea un manejador de tokens
                var jwtToken = handler.ReadJwtToken(token); // Se lee la token
                // Se accede a los claims de la token
                var userId = jwtToken.Claims.First(claim => claim.Type == "Id").Value;
                int IdUser = int.Parse(userId); // Se obtiene el id del usuario y se parsea a entero
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
                var purchase = _purchaseService.MakePurchase(id, quantity, IdUser).Result;
                
                return Ok(_mapper.PurchaseToPurchaseDTO(purchase));
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("searchPurchase/{searchString}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchPurchase(string searchString)
        {
            try
            {
                var products = await _purchaseService.SearchPurchase(searchString);
                if (!products.Any())
                {
                    return NotFound("No se encontraron productos que coincidan con la búsqueda.");
                }
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}