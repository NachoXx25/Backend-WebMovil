using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(); //Se obtienen todos los productos

        Task<string> AddProduct(ProductDTO productDTO); //Se obtiene un producto

        Task<UpdateProductDTO> UpdateProduct(int id, UpdateProductDTO productDTO); //Se actualiza un producto

        Task<Product?> GetProductById(int id); //Obtiene el producto por id

        Task<ProductDTO?> GetProductByNameAndType(ProductDTO productDTO); //Se obtiene un producto por nombre y tipo

        Task<IEnumerable<Product>> GetAvailableProducts(); //Se obtienen los productos disponibles

        Task VerifyNameAndType(ProductDTO productDTO); //Se verifica el nombre del producto
 
        Task DeleteProduct(int id); //Se elimina un producto

        Task<IEnumerable<ProductDTO>> SearchProducts(string searchString);
    }
}