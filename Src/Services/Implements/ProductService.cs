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

        private readonly IPhotoService _photoService;

        public ProductService(IProductRepository repository, IMapperService mapperService, IPhotoService photoService)
        {
            _repository = repository;
            _mapperService = mapperService;
            _photoService = photoService;
        }
        public async Task<string> AddProduct(ProductDTO productDTO, IFormFile photo)
        {
            if (photo.Length > 0)
            {
                var imageUploadResult = await _photoService.AddPhoto(photo);
                productDTO.Image = imageUploadResult.Url.ToString();
            }
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

        public async Task<ProductDTO?> GetProductByNameAndType(UpdateProductDTO productDTO)
        {
            var product = await _repository.GetProductByNameAndType(productDTO.Name, productDTO.Type);
            return _mapperService.ProductToProductDTO(product);
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetProducts().Result;
            return Task.FromResult(products);
        }

        public async Task<UpdateProductDTO> UpdateProduct(int id, UpdateProductDTO productDTO, IFormFile? photo)
        {
            {   
                if(photo != null){
                    var result = await _photoService.AddPhoto(photo);
                    if (result != null){
                        productDTO.Image = result.Url.ToString();
                    }
                }
                var product = await _repository.GetProductById(id);
                if (product == null)
                {
                    throw new Exception("Producto no encontrado");
                }
                await GetProductByNameAndType(productDTO);
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
                if (productDTO.Price.HasValue)
                {
                    product.Price = productDTO.Price.Value;
                }
                if (productDTO.Stock.HasValue)
                {
                    product.Stock = productDTO.Stock.Value;
                }
                if (!string.IsNullOrEmpty(productDTO.Image))
                {
                    var imageUploadResult = await _photoService.AddPhoto(photo);
                    product.Image = imageUploadResult.Url.ToString() ?? product.Image;
                }
                await _repository.SaveChanges();
                var updateProductDTO = _mapperService.ProductToUpdateProductDTO(product);
                return updateProductDTO;
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

        public async Task<IEnumerable<ProductDTO>> SearchProducts(string searchString)
        {
            var products = await _repository.GetProducts();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Realizar búsqueda por nombre y tipo
                products = products.Where(p =>
                    p.Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase) ||
                    p.Type.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)
                );
            }

            return products.Select(p => _mapperService.ProductToProductDTO(p));
        }

    }
}