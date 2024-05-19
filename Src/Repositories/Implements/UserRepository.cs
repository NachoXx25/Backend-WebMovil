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
                users = users.Where(p =>
                    p.Id.ToString().Equals(searchString) ||
                    p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Rut.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    p.BirthDate.Day.ToString().Contains(searchString) ||
                    p.BirthDate.Month.ToString().Contains(searchString) ||
                    p.BirthDate.Year.ToString().Contains(searchString) ||
                    p.Active.ToString().Equals(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList(); // Filtrar los usuarios por nombre, rut, email, género, fecha de nacimiento, activo, Id
            }

            return users; // Retornar los usuarios filtrados
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