using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(); //Se obtienen todos los productos

        Task<Product?> GetProductById(int id); //Se obtiene un producto mediante su id

        Task<bool> DeleteProduct(Product product); //Se elimina un producto

        Task AddProduct(Product product); //Se agrega un producto
        

        Task<IEnumerable<Product>> AvailableProducts(); //Se obtienen los productos disponibles

        Task<IEnumerable<Product>> SearchProducts(string search); //Obtener todos los productos filtrandolos por caracteristicas especificas, stock > 0

        Task<IEnumerable<Product>> AdminSearchProducts(string search); //Obtener todos los productos filtrandolos por caracteristicas especificas

        Task<Product?> GetProductByNameAndType(string name, string type); //Se obtiene un producto mediante su nombre y tipo
        public Task SaveChanges(); //Se guardan los cambios en la base de datos
    }
}