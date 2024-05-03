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
                throw new Exception("Producto ya existe");
            }

            await _repository.AddProduct(mappedProduct);
            return "Producto agregado";
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product == null)
            {
                throw new Exception("Producto no encontrado");
            }
            await _repository.DeleteProduct(product);
            await _repository.SaveChanges();
        }

        public Task<IEnumerable<Product>> GetAvailableProducts()
        {
            var products = _repository.AvailableProducts().Result;
            return Task.FromResult(products);
        }

        public Task<Product?> GetProductById(int id)
        {
            var product = _repository.GetProductById(id).Result;
            return Task.FromResult(product);
        }

        public async Task<ProductDTO?> GetProductByNameAndType(ProductDTO productDTO)
        {
            var product = await _repository.GetProductByNameAndType(productDTO.Name, productDTO.Type);
            return _mapperService.ProductToProductDTO(product);
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetProducts().Result;
            return Task.FromResult(products);
        }

        public async Task UpdateProduct(int id, UpdateProductDTO productDTO)
        {
            {
                var product = await _repository.GetProductById(id);
                if (product == null)
                {
                    throw new Exception("Producto no encontrado");
                }
                var productDTOToCompare = _mapperService.UpdateProductDTOToProduct(productDTO);
                await GetProductByNameAndType(productDTOToCompare);
                if(!string.IsNullOrEmpty(productDTO.Name))
                {
                    product.Name = productDTO.Name ?? product.Name;
                }
                if(!string.IsNullOrEmpty(productDTO.Type))
                {
                    var categorias = new string[] {"Tecnología", "Electrohogar", "Juguetería", "Ropa", "Muebles", "Comida", "Libros"};
                    if (!categorias.Any(categoria => categoria.Equals(productDTO.Type, StringComparison.Ordinal)))
                    {
                        throw new Exception("Categoría no válida (categorías válidas: Tecnología, Electrohogar, Juguetería, Ropa, Muebles, Comida, Libros)");
                    }
                    product.Type = productDTO.Type ?? product.Type;
                }
                if(productDTO.Price.HasValue)
                {
                    product.Price = productDTO.Price.Value;
                }
                if(productDTO.Stock.HasValue)
                {
                    product.Stock = productDTO.Stock.Value ;
                }
                if(!string.IsNullOrEmpty(productDTO.Image))
                {
                    product.Image = productDTO.Image ?? product.Image;
                }
                await _repository.SaveChanges();
            }
        }

        public async Task VerifyNameAndType(ProductDTO productDTO)
        {
            var nameToCompare = productDTO.Name.Replace(" ", "").ToUpperInvariant();
            var typeToCompare = productDTO.Type.Replace(" ", "").ToUpperInvariant();
            var allProducts = await _repository.GetProducts();
            var product = allProducts.FirstOrDefault(p => p.Name.Replace(" ", "").ToUpperInvariant() == nameToCompare 
                                                  && p.Type.Replace(" ", "").ToUpperInvariant() == typeToCompare);
            if (product != null)
            {
                throw new Exception("Producto ya existe");
            }
            return;
        }
    }
}