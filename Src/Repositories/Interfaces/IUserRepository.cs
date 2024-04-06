using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IUserRepository
    {
         
        Task<IEnumerable<User>> GetUsers();

        Task<User?> GetUserByEmail(string Email);

        Task<bool> VerifyUserByEMail(string Email);

        Task AddUser(User user);

        Task<bool> VerifyUserByRut(string Rut);
    }
}