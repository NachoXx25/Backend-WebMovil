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
        
        public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO) //mapeo de RegisterUserDTO a User
        {
            var mappedUser = _mapper.Map<User>(registerUserDTO);  
            return mappedUser; //retorna el objeto mapeado
        }

    }
}