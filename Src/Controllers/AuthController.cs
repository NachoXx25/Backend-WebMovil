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
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loqinUserDTO) //metodo para loguear un usuario
        {
            var result = await _authService.LoginUser(loqinUserDTO); //se llama al metodo de login
            
            if (result != null){
                return Ok(result); //se retorna el token
            }
            return BadRequest("Credenciales inválidas."); //si no se logra loguear, se retorna un mensaje de credenciales inválidas
        }
    }
}