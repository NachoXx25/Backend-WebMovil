using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loginUserDTO) //metodo para loguear un usuario
        {
            var result = await _authService.LoginUser(loginUserDTO); //se llama al metodo de login
            if (result != null)
            {
                // Crear la cookie de token
                Response.Cookies.Append("token", result, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(1) // Expira en 1 hora
                });
                return Ok(result); //se retorna el token
            }
            return BadRequest("Credenciales inválidas."); //si no se logra loguear, se retorna un mensaje de credenciales inválidas
        }


        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser(RegisterUserDTO registerUserDTO) //metodo para registrar un usuario
        {
            try
            {
                var result = await _authService.RegisterUser(registerUserDTO); //se llama al metodo de registro
                return Ok(result); //se retorna el token
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); //si no se logra registrar, se retorna un mensaje de error, los de errores de servidor
            }
        }

        [HttpGet("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            // Eliminar la cookie de token
            Response.Cookies.Delete("token");
            return Ok("Sesión cerrada exitosamente.");
        }
    }
}