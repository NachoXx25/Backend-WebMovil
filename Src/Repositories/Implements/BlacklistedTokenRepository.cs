using Microsoft.EntityFrameworkCore;
using taller1WebMovil.Src.Data;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

public class BlacklistedTokenRepository : IBlacklistedTokenRepository
{
    private readonly DataContext _context;

    public BlacklistedTokenRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddTokenToBlacklist(int userId, string token)
    {
        var blacklistedToken = new BlacklistedToken
        {
            UserId = userId,
            Token = token
        };

        await _context.BlacklistedToken.AddAsync(blacklistedToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenInBlacklist(string token)
    {
        return await _context.BlacklistedToken.AnyAsync(t => t.Token == token);
    }

    public async Task RemoveTokenFromBlacklist(int userId)
    {
        var tokensToRemove = await _context.BlacklistedToken.Where(t => t.UserId == userId).ToListAsync();
        _context.BlacklistedToken.RemoveRange(tokensToRemove);
        await _context.SaveChangesAsync();
    }
}