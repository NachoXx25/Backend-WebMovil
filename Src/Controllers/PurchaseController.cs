using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapperService _mapper;
        public PurchaseController(IProductService productService, IMapperService mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet ("all")]
        public ActionResult<ProductDTO> GetAvailableProducts(){
            var products = _productService.GetAvailableProducts().Result;
            var productDTOs = products.Select(p => _mapper.ProductToProductDTO(p)).ToList();
            if(productDTOs == null){
                return NotFound("No se encontraron productos");
            }
            return Ok(productDTOs);
        }
    }
}