using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> AddProduct(ProductDTO productDTO, IFormFile photo); //Se obtiene un producto

        Task<UpdateProductDTO> UpdateProduct(int id, UpdateProductDTO productDTO, IFormFile photo); //Se actualiza un producto

        Task<Product?> GetProductById(int id); //Obtiene el producto por id

        Task<ProductDTO?> GetProductByNameAndType(UpdateProductDTO productDTO); //Se obtiene un producto por nombre y tipo

        Task VerifyNameAndType(ProductDTO productDTO); //Se verifica el nombre del producto
 
        Task DeleteProduct(int id); //Se elimina un producto

        Task<IEnumerable<ProductDTO>> ClienteSearchProducts(string searchString); //Se buscan productos

        Task<IEnumerable<ProductDTO>> AdminSearchProducts(string searchString); //Se buscan productos

    }
}