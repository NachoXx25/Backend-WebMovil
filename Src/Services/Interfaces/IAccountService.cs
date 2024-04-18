
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetNonAdminUsers(); //Se obtienen todos los usuarios

        Task<User> DisableAccount(string rut); //Se deshabilita una cuenta

        Task<User> EnableAccount(string rut); //Se habilita una cuenta

    }
}