using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IRoleRepository
    {
         Task<Role?> GetRoleById(int id); //Se obtiene el rol mediante su Id

         Task<Role?> GetRoleByName(string name); //Se obtiene el rol mediante su nombre
    }

    
}