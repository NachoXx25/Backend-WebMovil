using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context; //Inyección de dependencia

        public RoleRepository(DataContext dataContext)
        {
            _context = dataContext; 
        }
        public async Task<Role?> GetRoleById(int id) //Método para obtener un rol por su id
        {
            var result = await _context.Roles.FindAsync(id); //Se busca el rol en la base de datos, puede ser nulo
            return result; //Se retorna el rol encontrado
        }

    }
}