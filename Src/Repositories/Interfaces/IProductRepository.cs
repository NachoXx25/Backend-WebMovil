using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(); //Se obtienen todos los productos

        Task<Product?> GetProductById(int id); //Se obtiene un producto mediante su id

        Task<bool> DeleteProduct(Product product); //Se elimina un producto

        Task AddProduct(Product product); //Se agrega un producto
        
        Task UpdateProduct(Product product,ProductDTO productdto); //Se actualiza un producto

        Task<Product?> GetProductByNameAndType(string name, string type); //Se obtiene un producto mediante su nombre y tipo
        public Task SaveChanges();
    }
}