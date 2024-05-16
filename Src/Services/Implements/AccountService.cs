using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repository; //Inyección de dependencia

        private readonly IMapperService _mapperService; //Inyección de dependencia

        public AccountService(IUserRepository repository, IMapperService mapperService)
        {
            _repository = repository;

            _mapperService = mapperService;
        }

        public Task<User> DisableAccount(string rut)
        {   
            rut = FormatRut(rut); //Se formatea el rut
            var users = _repository.GetUserByRut(rut).Result; //Se obtiene el usuario por su rut

            if(users == null){ //Si no se encuentra el usuario
                throw new System.Exception("No se encontró el usuario."); //Se lanza una excepción 
            }
            if(users.Active == false){ //Si el usuario ya está desactivado
                throw new System.Exception("El usuario ya se encuentra desactivado."); //Se lanza una excepción
            }
            if(users.RoleId == 1){ //Si el usuario es administrador
                throw new System.Exception("No se puede desactivar un usuario administrador."); //Se lanza una excepción
            }
            users.Active = false; //Se desactiva el usuario
            _repository.SaveChanges(); //Se guardan los cambios en la base de datos
            return Task.FromResult(users); //Se retorna el usuario
        }

        public Task<User> EnableAccount(string rut)
        {
            rut = FormatRut(rut); //Se formatea el rut
            var users = _repository.GetUserByRut(rut).Result; //Se obtiene el usuario por su rut
            if(users == null){ //Si no se encuentra el usuario
                throw new System.Exception("No se encontró el usuario."); //Se lanza una excepción
            }
            if(users.Active == true){ //Si el usuario ya está activo
                throw new System.Exception("El usuario ya se encuentra activo."); //Se lanza una excepción
            }
            if(users.RoleId == 1){ //Si el usuario es administrador
                throw new System.Exception("No se puede activar un usuario administrador."); //Se lanza una excepción
            }
            users.Active = true; //Se activa el usuario
            _repository.SaveChanges(); //Se guardan los cambios en la base de datos
            return Task.FromResult(users); //Se retorna el usuario
        }

        public Task<IEnumerable<User>> GetNonAdminUsers()
        {
            var users = _repository.GetUsers().Result; //Se obtienen todos los usuarios
            var nonAdminUsers = users.Where(user => user.RoleId == 2).ToList(); //Se obtienen los usuarios que no son administradores
            return Task.FromResult<IEnumerable<User>>(nonAdminUsers); //Se retornan los usuarios
        }
        public static string FormatRut(string rut)
        {
            int cont = 0; //Se crea un contador
            string format;//Se crea un string format

            rut = rut.Replace(".", ""); //Se reemplaza el punto por un espacio vacío
            rut = rut.Replace("-", ""); // Se reemplaza el guión por un espacio vacío
            format = "-" + rut.Substring(rut.Length - 1); //Se obtiene el último dígito del rut
            for (int i = rut.Length - 2; i >= 0; i--)
            {
                format = rut.Substring(i, 1) + format; //Se obtiene el dígito y se agrega al formato
                cont++; //Se aumenta el contador
                if (cont == 3 && i != 0) //Si el contador es igual a 3 y no es el último dígito
                {
                    format = "." + format; //Se agrega un punto al formato
                    cont = 0; //Se reinicia el contador
                }
            }
            return format; //Se retorna el rut formateado
        }

        public async Task<bool> EditUser(int userId, UserProfileEditDTO userProfileEditDTO)
            {
                try
                {
                    var user = await _repository.GetUserById(userId); // Obtener el usuario por ID

                    if (user == null)
                    {
                        return false; // Si no se encuentra el usuario, retornar falso
                    }

                    if (!string.IsNullOrEmpty(userProfileEditDTO.Name)) // Si el nombre no es nulo o vacío
                    {
                        user.Name = userProfileEditDTO.Name; // Actualizar el nombre
                    }

                    if (userProfileEditDTO.BirthDate != default) // Si la fecha de nacimiento no es la predeterminada
                    {
                        user.BirthDate = userProfileEditDTO.BirthDate; // Actualizar la fecha de nacimiento
                    }

                    if (!string.IsNullOrEmpty(userProfileEditDTO.Gender)) // Si el género no es nulo o vacío
                    {
                        user.Gender = userProfileEditDTO.Gender; // Actualizar el género
                    }
                    await _repository.SaveChanges(); // Guardar los cambios en la base de datos

                    return true; // Retornar verdadero
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al editar el usuario: {ex.Message}"); // Mostrar mensaje de error
                    return false; // Retornar falso
                }  
            }

        public async Task<bool> EditPassword(int userId, EditPasswordDTO editPasswordDTO)
        {
            try
            {
                var user = await _repository.GetUserById(userId); // Obtener el usuario por ID

                if (user == null) // Si no se encuentra el usuario
                {
                    Console.WriteLine($"No se encontró el usuario con ID: {userId}"); // Mostrar mensaje de error
                    return false; // Retornar falso
                }

                if (string.IsNullOrEmpty(editPasswordDTO.Password)) // Si la contraseña es nula o vacía
                {
                    return false;
                }

                if (editPasswordDTO.NewPassword != editPasswordDTO.ConfirmPassword) // Si la nueva contraseña no coincide con la confirmación
                {
                    return false;
                }

                var result = BCrypt.Net.BCrypt.Verify(editPasswordDTO.Password, user.Password); // Verificar la contraseña

                if (!result)
                {
                    return false;
                }

                var result2 = BCrypt.Net.BCrypt.Verify(editPasswordDTO.NewPassword, user.Password); // Verificar la nueva contraseña

                if (result2) // Si la nueva contraseña es igual a la anterior
                {
                    Console.WriteLine($"Contraseña igual a la anterior, no aceptada"); // Mostrar mensaje de error
                    return false;
                }

                var salt = BCrypt.Net.BCrypt.GenerateSalt(12); // Generar una nueva sal
                user.Password = BCrypt.Net.BCrypt.HashPassword(editPasswordDTO.NewPassword, salt); // Actualizar la contraseña
                await _repository.SaveChanges(); // Guardar los cambios en la base de datos

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar la contraseña: {ex.Message}"); // Mostrar mensaje de error
                return false;
            }
        }

        public async Task<IEnumerable<UserDTO>> SearchUsers(string searchString)
        {
            var users = await _repository.GetUsers(); // Obtener todos los usuarios
            
            if (!string.IsNullOrEmpty(searchString)) // Si la cadena de búsqueda no es nula o vacía
            {
                users = users.Where(p =>
                    p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Rut.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ); // Filtrar los usuarios por nombre, rut, email y género
            }

            return users.Select(p => _mapperService.UserToUserDTO(p)); // Mapear los usuarios a DTO
        }
    }
}