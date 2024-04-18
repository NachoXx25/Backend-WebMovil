using taller1WebMovil.Src.Models;
namespace taller1WebMovil.Src.Repositories.Interfaces
{
    public interface IBlacklistedTokenRepository
    {
    Task AddTokenToBlacklist(int userId, string token);
    Task<bool> IsTokenInBlacklist(string token);
    Task RemoveTokenFromBlacklist(int userId);
    }
}
