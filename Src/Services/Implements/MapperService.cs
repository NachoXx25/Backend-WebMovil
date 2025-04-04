using AutoMapper;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper; 
        public MapperService(IMapper mapper) //inyeccion de dependencias
        {
            _mapper = mapper;
        }

        public User EdiProfileDTOToUser(UserProfileEditDTO userEditDTO)
        {
            var mappedUserEditDTO = _mapper.Map<User> (userEditDTO); //mapeo de UserProfileEditDTO a User
            return mappedUserEditDTO; //retorna el objeto mapeado
        }

        public Product ProductDTOToProduct(ProductDTO productDTO)
        {
            var mappedProductDTO = _mapper.Map<Product>(productDTO); //mapeo de ProductDTO a Product
            return mappedProductDTO; //retorna el objeto mapeado
        }

        public ProductDTO ProductToProductDTO(Product product)
        {
            var mappedProduct = _mapper.Map<ProductDTO>(product); //mapeo de Product a ProductDTO
            return mappedProduct; //retorna el objeto mapeado
        }

        public UpdateProductDTO ProductToUpdateProductDTO(Product product)
        {
            var mappedUpdateProductDTO = _mapper.Map<UpdateProductDTO>(product); //mapeo de Product a UpdateProductDTO
            return mappedUpdateProductDTO; //retorna el objeto mapeado
        }

        public PurchaseDTO PurchaseToPurchaseDTO(Purchase purchase)
        {
            var mappedPurchase = _mapper.Map<PurchaseDTO>(purchase); //mapeo de Purchase a PurchaseDTO
            return mappedPurchase; //retorna el objeto mapeado
        }

        public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO) //mapeo de RegisterUserDTO a User
        {
            var mappedUser = _mapper.Map<User>(registerUserDTO);  //mapeo de RegisterUserDTO a User
            return mappedUser; //retorna el objeto mapeado
        }

        public ProductDTO UpdateProductDTOToProduct(UpdateProductDTO updateProductDTO)
        {
            var mappedUpdateProductDTO = _mapper.Map<ProductDTO>(updateProductDTO); //mapeo de UpdateProductDTO a ProductDTO
            return mappedUpdateProductDTO; //retorna el objeto mapeado
        }

        public UserDTO UserToUserDTO(User user)
        {
            var mappedUserDTO = _mapper.Map<UserDTO>(user);
            return mappedUserDTO; //retorna el objeto mapeado
        }
    }
}