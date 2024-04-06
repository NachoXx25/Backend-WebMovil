using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string Email)
        {
            var user = await _context.Users.Where(u => u.Email == Email)
                                            .Include(u => u.Role)
                                            .FirstOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> VerifyUserByEMail(string Email)
        {
            var user = await _context.Users.Where(u => u.Email == Email)
                                            .FirstOrDefaultAsync();
            if(user == null){
                return false;
            }
            return true;
            
        }

        public async Task<bool> VerifyUserByRut(string Rut)
        {
            var user = await _context.Users.Where(u => u.Rut == Rut)
                                            .FirstOrDefaultAsync();
            if(user == null){
                return false;
            }
            return true;
        }

    }
}