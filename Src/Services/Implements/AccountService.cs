using BCrypt.Net;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repository;

        private readonly IMapperService _mapperService;

        public AccountService(IUserRepository repository, IMapperService mapperService)
        {
            _repository = repository;

            _mapperService = mapperService;
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

        public async Task<bool> EditUser(int userId, UserProfileEditDTO userProfileEditDTO)
            {
                try
                {
                    var user = await _repository.GetUserById(userId);

                    if (user == null)
                    {
                        return false;
                    }

                    // Actualizar solo los campos que se proporcionan en el DTO
                    if (!string.IsNullOrEmpty(userProfileEditDTO.Name))
                    {
                        user.Name = userProfileEditDTO.Name;
                    }

                    if (userProfileEditDTO.BirthDate != default)
                    {
                        user.BirthDate = userProfileEditDTO.BirthDate;
                    }

                    if (!string.IsNullOrEmpty(userProfileEditDTO.Gender))
                    {
                        user.Gender = userProfileEditDTO.Gender;
                    }

                    // Guardar los cambios en la base de datos
                    await _repository.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    // Manejar excepciones aquí, por ejemplo, loguear el error
                    Console.WriteLine($"Error al editar el usuario: {ex.Message}");
                    return false;
                }  
            }

        public async Task<bool> EditPassword(int userId, EditPasswordDTO editPasswordDTO)
        {
            try
            {
                var user = await _repository.GetUserById(userId);

                if (user == null)
                {
                    Console.WriteLine($"No se encontró el usuario con ID: {userId}");
                    return false;
                }

                if (string.IsNullOrEmpty(editPasswordDTO.Password))
                {
                    return false;
                }

                if (editPasswordDTO.NewPassword != editPasswordDTO.ConfirmPassword)
                {
                    return false;
                }

                var result = BCrypt.Net.BCrypt.Verify(editPasswordDTO.Password, user.Password);

                if (!result)
                {
                    return false;
                }

                var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                user.Password = BCrypt.Net.BCrypt.HashPassword(editPasswordDTO.NewPassword, salt);
                await _repository.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar la contraseña: {ex.Message}");
                return false;
            }
        }
    }
}