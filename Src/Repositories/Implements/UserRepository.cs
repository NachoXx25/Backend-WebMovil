using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context; //Inyección de dependencia

        public UserRepository(DataContext context) //Inyección de dependencia
        {
            _context = context;
        }

        public async Task AddUser(User user) //Método para agregar un usuario
        {
            await _context.Users.AddAsync(user); //Se agrega el usuario a la base de datos
            await _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
        }

        public async Task<User?> GetUserByEmail(string Email) //Método para obtener un usuario por su email
        {
            var user = await _context.Users.Where(u => u.Email == Email)
                                            .Include(u => u.Role)
                                            .FirstOrDefaultAsync(); //Se busca el usuario en la base de datos, puede ser nulo
            return user; //Se retorna el usuario encontrado
        }

        public async Task<User?> GetUserById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id)
                                            .Include(u => u.Role)
                                            .FirstOrDefaultAsync(); //Se busca el usuario en la base de datos, puede ser nulo
            return user; //Se retorna el usuario encontrado
        }

        public Task<User?> GetUserByRut(string Rut)
        {
            var user = _context.Users.Where(u => u.Rut == Rut)
                                            .Include(u => u.Role)
                                            .FirstOrDefaultAsync(); //Se busca el usuario en la base de datos, puede ser nulo

            return user; //Se retorna el usuario encontrado
        }

        public async Task<IEnumerable<User>> GetUsers() //Método para obtener todos los usuarios
        {   
            var users = await _context.Users.ToListAsync(); //Se obtienen todos los usuarios de la base de datos
            return users; //Se retornan los usuarios encontrados
        }


        public Task SaveChanges()
        {
            return _context.SaveChangesAsync(); //Se guardan los cambios en la base de datos
        }

        public async Task<IEnumerable<User>> GetNonAdminUsers()
        {
            var users = await _context.Users.ToListAsync(); // Obtener todos los usuarios
            var nonAdminUsers = users.Where(user => user.RoleId == 2).ToList(); // Filtrar los usuarios que no son administradores
            return nonAdminUsers; // Retornar los usuarios no administradores
        }

        public async Task<IEnumerable<User>> SearchUsers(string searchString)
        {
            var users = await GetNonAdminUsers(); // Obtener todos los usuarios no admins
            if (!string.IsNullOrEmpty(searchString)) // Si la cadena de búsqueda no es nula o vacía
            {
                // Convertir la cadena de búsqueda en un objeto DateTime si es una fecha válida
                if (DateTime.TryParse(searchString, out DateTime searchDate))
                {
                    users = users.Where(p =>
                        p.BirthDate.Date == searchDate.Date // Comparar solo la parte de la fecha, ignorando la hora
                    ).ToList(); // Filtrar los usuarios por fecha de nacimiento
                }
                else
                {
                    // Si la cadena de búsqueda no es una fecha válida, tratarla como un RUT
                    string formattedRut = FormatRut(searchString);
                    users = users.Where(p =>
                        p.Id.ToString().Equals(searchString) ||
                        p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.Rut.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.Rut.Contains(formattedRut, StringComparison.OrdinalIgnoreCase) ||
                        p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.Active.ToString().Equals(searchString, StringComparison.OrdinalIgnoreCase)
                    ).ToList(); // Filtrar los usuarios por nombre, rut, email, género, activo, Id
                }
            }
            return users; // Retornar los usuarios filtrados
        }

        public static string FormatRut(string rut)
        {
            // Verificar si la cadena tiene el formato esperado de un RUT
            if (!Regex.IsMatch(rut, @"^(\d{1,8})-?(\d|k|K)$"))
            {
                // Si no tiene el formato de un RUT, devolver la cadena original sin modificar
                return rut;
            }

            // Remover puntos y guiones del RUT
            rut = rut.Replace(".", "").Replace("-", "");

            int cont = 0; //Se crea un contador
            string format = "-" + rut.Substring(rut.Length - 1); //Se obtiene el último dígito del rut

            // Formatear el RUT
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

        public async Task<bool> VerifyUserByEMail(string Email) //Método para verificar si un usuario existe por su email
        {
            var user = await _context.Users.Where(u => u.Email == Email)
                                            .FirstOrDefaultAsync(); //Se busca el usuario en la base de datos, puede ser nulo
            if(user == null){ //Si el usuario no existe
                return false; //Se retorna falso
            }
            return true; //Si el usuario existe, se retorna verdadero
            
        }

        public async Task<bool> VerifyUserByRut(string Rut) //Método para verificar si un usuario existe por su rut
        {
            var user = await _context.Users.Where(u => u.Rut == Rut)
                                            .FirstOrDefaultAsync(); //Se busca el usuario en la base de datos, puede ser nulo
            if(user == null){ //Si el usuario no existe
                return false; //Se retorna falso
            }
            return true; //Si el usuario existe, se retorna verdadero
        }

    }
}