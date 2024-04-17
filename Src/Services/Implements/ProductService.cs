using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<Product> AddProduct()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            await _repository.DeleteProduct(product);
            await _repository.SaveChanges();
        }

        public Task<Product?> GetProductById(int id)
        {
            var product = _repository.GetProductById(id).Result;
            return Task.FromResult(product);
        }


        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetProducts().Result;
            return Task.FromResult(products);
        }

        public Task<Product> UpdateProduct(int id)
        {
            throw new NotImplementedException();
        }

    }
}