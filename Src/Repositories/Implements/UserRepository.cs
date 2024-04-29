using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context; //Inyección de dependencia

        private readonly IRoleRepository _roleRepository;

        private readonly IConfiguration _configuration;

        public UserRepository(DataContext context, IRoleRepository roleRepository, IConfiguration configuration) //Inyección de dependencia
        {
            _context = context;
            _roleRepository = roleRepository;
            _configuration = configuration;
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

        public async Task<User?> GetUserById(int Id)
        {
            var user = await _context.Users.Where(u => u.Id == Id)
                                        .Include(u => u.Role)
                                        .FirstOrDefaultAsync();
            return user;
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