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
        private readonly IProductService _productService; //se inyecta el servicio de productos
        private readonly IMapperService _mapper; //se inyecta el servicio de mapeo

        private readonly IPurchaseService _purchaseService; //se inyecta el servicio de compras
        public PurchaseController(IProductService productService, IMapperService mapper, IPurchaseService purchaseService)
        {
            _productService = productService;
            _mapper = mapper;
            _purchaseService = purchaseService;
        }

        [Authorize(Roles = "User")]
        [HttpGet("searchProducts/{searchString?}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string searchString = null)
        {
            try
            {
                var products = await _productService.ClienteSearchProducts(searchString); //se llama al metodo de buscar productos
                if (!products.Any())
                {
                    return NotFound("No se encontraron productos que coincidan con la búsqueda."); //si no se logra encontrar, se retorna un mensaje de no encontrado
                }
                return Ok(products); //se retornan los productos
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); //si no se logra encontrar, se retorna un mensaje de error
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
                    return BadRequest("El id debe ser mayor a 0"); //si el id es menor o igual a 0, se retorna un mensaje de error
                }
                var product = _productService.GetProductById(id).Result; //se obtiene el producto por id
                if(product == null){
                    return NotFound("Producto no encontrado"); //si no se logra encontrar el producto, se retorna un mensaje de no encontrado
                }
                if(quantity <= 0){
                    return BadRequest("La cantidad debe ser mayor a 0"); //si la cantidad es menor o igual a 0, se retorna un mensaje de error
                }
                if(quantity > product.Stock){
                    return BadRequest("No hay suficiente stock"); //si la cantidad es mayor al stock, se retorna un mensaje de no hay suficiente stock
                }
                var purchase = _purchaseService.MakePurchase(id, quantity, IdUser).Result; //se llama al metodo de hacer compra
                
                return Ok(_mapper.PurchaseToPurchaseDTO(purchase)); //se retorna la compra
            }catch(Exception e){
                return BadRequest(e.Message); //si no se logra hacer la compra, se retorna un mensaje de error
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("searchPurchase/{searchString?}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchPurchase(string searchString = null)
        {
            try
            {
                var products = await _purchaseService.SearchPurchase(searchString); //se llama al metodo de buscar compras
                if (!products.Any())
                {
                    return NotFound("No se encontraron productos que coincidan con la búsqueda."); //si no se logra encontrar, se retorna un mensaje de no encontrado
                }
                return Ok(products); //se retornan los productos
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); //si no se logra encontrar, se retorna un mensaje de error
            }
        }
    }
}