using System.Diagnostics.Tracing;
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
        private readonly IProductService _productService;
        private readonly IMapperService _mapper;
        private readonly IPhotoService _photoService;

        public ProductController(IProductService productService, IMapperService mapper, IPhotoService photoService)
        {
            _productService = productService;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet("search/{searchString}")]
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



        [HttpPost("add")]
        public async Task<ActionResult<string>> AddProduct([FromForm] ProductDTO productDTO, IFormFile photo){
            
            try{ 
                await _productService.VerifyNameAndType(productDTO);
                var product = await _productService.AddProduct(productDTO, photo);
                if(product == null){
                    return NotFound("No se pudo agregar el producto");
                }
                return Ok(product);
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/update")]
        public async Task<ActionResult<UpdateProductDTO>> UptadeProduct(int id, [FromForm] UpdateProductDTO productDTO, IFormFile? photo){
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (productDTO == null)
                    {
                        throw new ArgumentNullException(nameof(productDTO));
                    }
                    var updateProductDTOs = await _productService.UpdateProduct(id, productDTO, photo);
                    return Ok(updateProductDTOs);
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
        }

        [HttpDelete("{id}/delete")]
        public async Task<ActionResult<string>> DeleteProduct(int id){
            var product = await _productService.GetProductById(id);
            if(product == null){
                return NotFound("No se encontró el producto.");
            }
            try{
                await _productService.DeleteProduct(id);
                return Ok("Producto eliminado.");
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

    }
}