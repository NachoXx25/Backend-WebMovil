using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(); //Se obtienen todos los productos

        Task<string> AddProduct(ProductDTO productDTO); //Se obtiene un producto

        Task<Product> UpdateProduct(int id); //Se actualiza un producto

        Task<Product?> GetProductById(int id); //Obtiene el producto por id
 
        Task DeleteProduct(int id); //Se elimina un producto
    }
}