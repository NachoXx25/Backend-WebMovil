using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context; //Inyección de dependencia

        public UserRepository(DataContext context)
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

        public async Task<IEnumerable<User>> GetUsers() //Método para obtener todos los usuarios
        {   
            var users = await _context.Users.ToListAsync(); //Se obtienen todos los usuarios de la base de datos
            users = users.Where(user => user.Rut != "20.416.699-4").ToList(); //hardcoding debido a que contamos con un unico administrador
            return users; //Se retornan los usuarios encontrados
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