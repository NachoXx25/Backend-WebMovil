using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService; //se inyecta el servicio de productos

        public ProductController(IProductService productService)
        {
            _productService = productService;

        }

        [HttpGet("search/{searchString?}")] 
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string searchString = null)
        {
            try
            {
                var products = await _productService.SearchProducts(searchString); //se llama al metodo de buscar productos
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



        [HttpPost("add")]
        public async Task<ActionResult<string>> AddProduct([FromForm] ProductDTO productDTO, IFormFile photo){
            
            try{ 
                await _productService.VerifyNameAndType(productDTO); //se llama al metodo de verificar nombre y tipo
                var product = await _productService.AddProduct(productDTO, photo); //se llama al metodo de agregar producto
                if(product == null){
                    return NotFound("No se pudo agregar el producto"); //si no se logra agregar, se retorna un mensaje de no encontrado
                }
                return Ok(product);
            }catch(Exception e){
                return BadRequest(e.Message); //si no se logra agregar, se retorna un mensaje de error
            }
        }

        [HttpPut("{id}/update")]
        public async Task<ActionResult<UpdateProductDTO>> UptadeProduct(int id, [FromForm] UpdateProductDTO productDTO, IFormFile? photo){
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState); //si el modelo no es valido, se retorna un mensaje de error
                    }
                    if (productDTO == null)
                    {
                        throw new ArgumentNullException(nameof(productDTO)); //si el producto es nulo, se lanza una excepción
                    }
                    var updateProductDTOs = await _productService.UpdateProduct(id, productDTO, photo); //se llama al metodo de actualizar producto
                    return Ok(updateProductDTOs); //se retorna el producto actualizado
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message); //si no se logra actualizar, se retorna un mensaje de error
                }
        }

        [HttpDelete("{id}/delete")]
        public async Task<ActionResult<string>> DeleteProduct(int id){
            var product = await _productService.GetProductById(id); //se obtiene el producto por id
            if(product == null){
                return NotFound("No se encontró el producto."); //si no se encuentra el producto, se retorna un mensaje de no encontrado
            }
            try{
                await _productService.DeleteProduct(id); //se llama al metodo de eliminar producto
                return Ok("Producto eliminado."); //se retorna un mensaje de eliminado
            }catch(Exception e){
                return BadRequest(e.Message); //si no se logra eliminar, se retorna un mensaje de error
            }
        }

    }
}