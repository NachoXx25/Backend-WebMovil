using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using taller1WebMovil.Src.Repositories.Interfaces; // Agregar esta directiva de uso

public class TokenBlacklistAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly IBlacklistedTokenRepository _blacklistedTokenRepository;
    private readonly ILogger<TokenBlacklistAuthorizationFilter> _logger; // Inyectar ILogger

    public TokenBlacklistAuthorizationFilter(IBlacklistedTokenRepository blacklistedTokenRepository, ILogger<TokenBlacklistAuthorizationFilter> logger)
    {
        _blacklistedTokenRepository = blacklistedTokenRepository;
        _logger = logger; // Asignar ILogger inyectado
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var token = ObtenerTokenDeSolicitud(context.HttpContext.Request);
        _logger.LogInformation($"Token obtenido: {token}"); // Registrar token obtenido

        if (await _blacklistedTokenRepository.IsTokenInBlacklist(token))
        {
            _logger.LogInformation($"El token está en la lista negra: {token}");
            context.Result = new ForbidResult();
        }
    }

    private string ObtenerTokenDeSolicitud(HttpRequest request)
    {
        return request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }
}