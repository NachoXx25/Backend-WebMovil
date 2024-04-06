using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loqinUserDTO)
        {
            var result = await _authService.LoginUser(loqinUserDTO);
            
            if (result != null){
                return Ok(result);
            }
            return BadRequest("Credenciales inválidas.");
        }
    }
}