using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IUserRepository
    {
         
        Task<IEnumerable<User>> GetUsers(); //Se obtienen todos los usuarios

        Task<User?> GetUserByEmail(string Email); //Se obtiene el usuario mediante su Email

        Task<bool> VerifyUserByEMail(string Email); //Se verifica si un usuario existe mediante su Email

        Task AddUser(User user); //Se agrega un usuario

        Task<bool> VerifyUserByRut(string Rut); //Se verifica si un usuario existe mediante su Rut
    }
}