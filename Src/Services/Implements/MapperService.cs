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

        public Product ProductDTOToProduct(ProductDTO productDTO)
        {
            var mappedProductDTO = _mapper.Map<Product>(productDTO);
            return mappedProductDTO; //retorna el objeto mapeado
        }

        public ProductDTO ProductToProductDTO(Product product)
        {
            var mappedProduct = _mapper.Map<ProductDTO>(product);
            return mappedProduct; //retorna el objeto mapeado
        }

        public PurchaseDTO PurchaseToPurchaseDTO(Purchase purchase)
        {
            var mappedPurchase = _mapper.Map<PurchaseDTO>(purchase);
            return mappedPurchase; //retorna el objeto mapeado
        }

        public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO) //mapeo de RegisterUserDTO a User
        {
            var mappedUser = _mapper.Map<User>(registerUserDTO);  
            return mappedUser; //retorna el objeto mapeado
        }

        public UserDTO UserToUserDTO(User user)
        {
            var mappedUserDTO = _mapper.Map<UserDTO>(user);
            return mappedUserDTO; //retorna el objeto mapeado
        }
    }
}