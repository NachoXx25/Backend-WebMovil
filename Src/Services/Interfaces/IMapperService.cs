using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IMapperService
    {
         public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO); //mapeo de RegisterUserDTO a User

         //public Product ProductDTOToProduct(ProductDTO productDTO); //mapeo de ProductDTO a Product
    }
}