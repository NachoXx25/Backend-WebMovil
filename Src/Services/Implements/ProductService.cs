using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        private readonly IMapperService _mapperService;

        public ProductService(IProductRepository repository, IMapperService mapperService)
        {
            _repository = repository;
            _mapperService = mapperService;
        }
        public async Task<string> AddProduct(ProductDTO productDTO)
        {
            var mappedProduct = _mapperService.ProductDTOToProduct(productDTO); 

            if (await _repository.GetProductByNameAndType(mappedProduct.Name, mappedProduct.Type) != null)
            {
                throw new Exception("Product already exists");
            }

            await _repository.AddProduct(mappedProduct);
            return "Producto agregado";
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