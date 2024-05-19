using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context; //Se crea un atributo de tipo DataContext

        public RoleRepository(DataContext dataContext)
        {
            _context = dataContext; 
        }
        public async Task<Role?> GetRoleById(int id) //Método para obtener un rol por su id
        {
            var result = await _context.Roles.FindAsync(id); //Se busca el rol en la base de datos, puede ser nulo
            return result; //Se retorna el rol encontrado
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            var role = await _context.Roles.Where(n => n.Name == name).FirstOrDefaultAsync(); //Se busca el rol en la base de datos, puede ser nulo}
            return role; //Se retorna el rol encontrado
        }

    }
}