using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IMapperService
    {
         public User RegisterUserDTOToUser(RegisterUserDTO registerUserDTO);
    }
}