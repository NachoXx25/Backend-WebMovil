using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IMapperService
    {
         public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO); //mapeo de RegisterUserDTO a User

         //public Product ProductDTOToProduct(ProductDTO productDTO); //mapeo de ProductDTO a Product

         public UserDTO UserToUserDTO(User user); //mapeo de User a UserDTO

         public ProductDTO ProductToProductDTO(Product product); //mapeo de Product a ProductDTO

         public Product ProductDTOToProduct(ProductDTO productDTO); //mapeo de ProductDTO a Product

         public PurchaseDTO PurchaseToPurchaseDTO(Purchase purchase); //mapeo de Purchase a PurchaseDTO

        public ProductDTO UpdateProductDTOToProduct(UpdateProductDTO updateProductDTO); //mapeo de UpdateProductDTO a Product

        public UpdateProductDTO ProductToUpdateProductDTO(Product product); //Mapeo de product a updateProductDTO
    }
}