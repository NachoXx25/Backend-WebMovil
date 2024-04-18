using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;


namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IBlacklistedTokenRepository _blacklistedTokenRepository;

        public AuthController(IUserRepository userRepository, IAuthService authService, IBlacklistedTokenRepository blacklistedTokenRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
            _blacklistedTokenRepository = blacklistedTokenRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loginUserDTO) //metodo para loguear un usuario
        {
            var result = await _authService.LoginUser(loginUserDTO); //se llama al metodo de login
            if (result != null){
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

        [HttpPost("logout")]
        [ServiceFilter(typeof(TokenBlacklistAuthorizationFilter))] 
        public async Task<ActionResult<string>> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = await _userRepository.ObtenerUserIdPorToken(token);
            await _blacklistedTokenRepository.AddTokenToBlacklist(userId, token);
            return Ok("Sesión cerrada exitosamente.");
        }
    }
}