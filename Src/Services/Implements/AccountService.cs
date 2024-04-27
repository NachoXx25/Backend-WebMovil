using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repository;

        public AccountService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<User> DisableAccount(string rut)
        {   
            rut = FormatRut(rut);
            var users = _repository.GetUserByRut(rut).Result;

            if(users == null){
                throw new System.Exception("No se encontró el usuario.");
            }
            if(users.Active == false){
                throw new System.Exception("El usuario ya se encuentra desactivado.");
            }
            if(users.RoleId == 1){
                throw new System.Exception("No se puede desactivar un usuario administrador.");
            }
            users.Active = false;
            _repository.SaveChanges();
            return Task.FromResult(users);
        }

        public Task<User> EnableAccount(string rut)
        {
            rut = FormatRut(rut);
            var users = _repository.GetUserByRut(rut).Result;
            if(users == null){
                throw new System.Exception("No se encontró el usuario.");
            }
            if(users.Active == true){
                throw new System.Exception("El usuario ya se encuentra activo.");
            }
            if(users.RoleId == 1){
                throw new System.Exception("No se puede activar un usuario administrador.");
            }
            users.Active = true;
            _repository.SaveChanges();
            return Task.FromResult(users);
        }

        public Task<IEnumerable<User>> GetNonAdminUsers()
        {
            var users = _repository.GetUsers().Result;
            var nonAdminUsers = users.Where(user => user.RoleId == 2).ToList();
            return Task.FromResult<IEnumerable<User>>(nonAdminUsers);
        }
        public static string FormatRut(string rut)
        {
            int cont = 0;
            string format;

            rut = rut.Replace(".", "");
            rut = rut.Replace("-", "");
            format = "-" + rut.Substring(rut.Length - 1);
            for (int i = rut.Length - 2; i >= 0; i--)
            {
                format = rut.Substring(i, 1) + format;
                cont++;
                if (cont == 3 && i != 0)
                {
                    format = "." + format;
                    cont = 0;
                }
            }
            return format;
        }
    }
}