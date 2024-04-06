using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<Role?> GetRoleById(int id)
        {
            var result = await _context.Roles.FindAsync(id);
            return result;
        }

    }
}