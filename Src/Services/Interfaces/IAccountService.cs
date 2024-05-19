
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IAccountService
    {
        Task<User> DisableAccount(string rut); //Se deshabilita una cuenta

        Task<User> EnableAccount(string rut); //Se habilita una cuenta

        Task<bool> EditUser(int userId, UserProfileEditDTO userProfileEditDTO); //Se edita un usuario

        Task<bool> EditPassword(int userId, EditPasswordDTO editPasswordDTO); //Se edita la contraseña de un usuario

        Task<IEnumerable<UserDTO>> SearchUsers(string searchString); //Se buscan usuarios
    }
}