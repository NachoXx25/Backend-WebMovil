using taller1WebMovil.Src.DTOs;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IAuthService
    {
         Task<string> RegisterUser(RegisterUserDTO registerUserDTO); 

         Task<string> LoginUser(LoginUserDTO loginUserDTO);
    }
}