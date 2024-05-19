using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository; //Inyección de dependencia

        private readonly IMapperService _mapperService; //Inyección de dependencia

        private readonly IPhotoService _photoService; //Inyección de dependencia

        public ProductService(IProductRepository repository, IMapperService mapperService, IPhotoService photoService)
        {
            _repository = repository;
            _mapperService = mapperService;
            _photoService = photoService;
        }
        public async Task<string> AddProduct(ProductDTO productDTO, IFormFile photo)
        {
            if (photo.Length > 0) //Se verifica que la imagen tenga contenido
            {
                var imageUploadResult = await _photoService.AddPhoto(photo); //Se sube la imagen a Cloudinary
                productDTO.Image = imageUploadResult.Url.ToString(); //Se obtiene la URL de la imagen
            }
            var mappedProduct = _mapperService.ProductDTOToProduct(productDTO); //Se mapea el DTO a un objeto de tipo Product

            if (await _repository.GetProductByNameAndType(mappedProduct.Name, mappedProduct.Type) != null)
            {
                throw new Exception("Producto ya existe"); //Se verifica si el producto ya existe
            }

            await _repository.AddProduct(mappedProduct); //Se agrega el producto
            return "Producto agregado"; //Se retorna un mensaje de éxito
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _repository.GetProductById(id); //Se obtiene el producto por su id
            if (product == null) 
            {
                throw new Exception("Producto no encontrado"); //Si el producto no existe, se lanza una excepción
            }
            await _repository.DeleteProduct(product); //Se elimina el producto
            await _repository.SaveChanges(); //Se guardan los cambios en la base de datos
        }

        public Task<Product?> GetProductById(int id)
        {
            var product = _repository.GetProductById(id).Result; //Se obtiene el producto por su id
            return Task.FromResult(product); //Se retorna el producto
        }

        public async Task<ProductDTO?> GetProductByNameAndType(UpdateProductDTO productDTO)
        {
            var product = await _repository.GetProductByNameAndType(productDTO.Name, productDTO.Type); //Se obtiene el producto por su nombre y tipo
            return _mapperService.ProductToProductDTO(product); //Se retorna el producto mapeado
        }

        public async Task<UpdateProductDTO> UpdateProduct(int id, UpdateProductDTO productDTO, IFormFile? photo)
        {
            {   
                if(photo != null){
                    var result = await _photoService.AddPhoto(photo); //Se sube la imagen a Cloudinary
                    if (result != null){
                        productDTO.Image = result.Url.ToString(); //Se obtiene la URL de la imagen
                    }
                }
                var product = await _repository.GetProductById(id); //Se obtiene el producto por su id
                if (product == null)
                {
                    throw new Exception("Producto no encontrado"); //Si el producto no existe, se lanza una excepción
                }
                await GetProductByNameAndType(productDTO); //Se verifica si el producto ya existe
                if(!string.IsNullOrEmpty(productDTO.Name)) //Si el nombre no es nulo o vacío
                {
                    product.Name = productDTO.Name ?? product.Name; //Se asigna el nuevo nombre al producto
                }
                if(!string.IsNullOrEmpty(productDTO.Type)) //Si el tipo no es nulo o vacío
                {
                    var categorias = new string[] {"Tecnología", "Electrohogar", "Juguetería", "Ropa", "Muebles", "Comida", "Libros"}; //Se crean las categorías válidas
                    if (!categorias.Any(categoria => categoria.Equals(productDTO.Type, StringComparison.Ordinal))) //Se verifica si la categoría es válida
                    {
                        throw new Exception("Categoría no válida (categorías válidas: Tecnología, Electrohogar, Juguetería, Ropa, Muebles, Comida, Libros)"); //Si la categoría no es válida, se lanza una excepción
                    }
                    product.Type = productDTO.Type ?? product.Type; //Se asigna el nuevo tipo al producto
                }
                if (productDTO.Price.HasValue) //Si el precio no es nulo
                {
                    product.Price = productDTO.Price.Value; //Se asigna el nuevo precio al producto
                }
                if (productDTO.Stock.HasValue) //Si el stock no es nulo
                {
                    product.Stock = productDTO.Stock.Value; //Se asigna el nuevo stock al producto
                }
                if (!string.IsNullOrEmpty(productDTO.Image)) //Si la imagen no es nula o vacía
                {
                    var imageUploadResult = await _photoService.AddPhoto(photo); //Se sube la imagen a Cloudinary
                    product.Image = imageUploadResult.Url.ToString() ?? product.Image; //Se asigna la nueva imagen al producto
                }
                await _repository.SaveChanges(); //Se guardan los cambios en la base de datos
                var updateProductDTO = _mapperService.ProductToUpdateProductDTO(product); //Se mapea el producto a un DTO
                return updateProductDTO; //Se retorna el DTO
            }
        }

        public async Task VerifyNameAndType(ProductDTO productDTO)
        {
            var nameToCompare = productDTO.Name.Replace(" ", "").ToUpperInvariant(); //Se obtiene el nombre del producto y se formatea
            var typeToCompare = productDTO.Type.Replace(" ", "").ToUpperInvariant(); //Se obtiene el tipo del producto y se formatea
            var allProducts = await _repository.GetProducts(); //Se obtienen todos los productos
            var product = allProducts.FirstOrDefault(p => p.Name.Replace(" ", "").ToUpperInvariant() == nameToCompare 
                                                  && p.Type.Replace(" ", "").ToUpperInvariant() == typeToCompare); //Se obtiene el producto por su nombre y tipo
            if (product != null)
            {
                throw new Exception("Producto ya existe"); //Si el producto ya existe, se lanza una excepción
            }
            return;
        }

        public async Task<IEnumerable<ProductDTO>> ClienteSearchProducts(string searchString)
        {
            var products = await _repository.SearchProducts(searchString); // Obtener los productos filtrados
            return products.Select(p => _mapperService.ProductToProductDTO(p)); // Mapear los productos a DTOs
        }

        public async Task<IEnumerable<ProductDTO>> AdminSearchProducts(string searchString)
        {
            var products = await _repository.AdminSearchProducts(searchString); // Obtener los productos filtrados
            return products.Select(p => _mapperService.ProductToProductDTO(p)); //Se mapean los productos a DTOs
        }

    }
}