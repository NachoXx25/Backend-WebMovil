using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MovilController : ControllerBase
    {
        private readonly IAuthService _authService;  //se inyecta el servicio de autenticación
        private readonly IUserRepository _userRepository; //se inyecta el repositorio de usuarios

        private readonly IPurchaseService _purchaseService;//se inyecta el servicio de compras

        public MovilController(IUserRepository userRepository, IAuthService authService, IPurchaseService purchaseService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _purchaseService = purchaseService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loginUserDTO)
        {
            var userClient = await _userRepository.GetUserByEmail(loginUserDTO.Email); //se obtiene el usuario por email

            if (userClient != null)
            {
                if (userClient.Role != null && userClient.Role.Name != "User") //Verificar si el usuario es un cliente
                {
                    return BadRequest("Solo se permite el uso a clientes");
                }

                var result = await _authService.LoginUser(loginUserDTO); //se llama al metodo de login

                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Credenciales inválidas."); //si no se logra loguear, se retorna un mensaje de credenciales inválidas
            }
            return BadRequest("Usuario no encontrado."); //si no se logra encontrar el usuario, se retorna un mensaje de usuario no encontrado
        }

        [Authorize(Roles = "User")]
        [HttpGet("tickets")]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> Tickets()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Split(' ')[1]; // Se obtiene el token
                var handler = new JwtSecurityTokenHandler(); // Se crea un manejador de tokens
                var jwtToken = handler.ReadJwtToken(token); // Se lee la token
                // Se accede a los claims de la token
                var userId = jwtToken.Claims.First(claim => claim.Type == "Id").Value;
                // Obtener las boletas del usuario utilizando el servicio de compras
                var tickets = await _purchaseService.SearchTicket(userId); //se llama al metodo de buscar boletas
                if (!tickets.Any())
                {
                    return NotFound("No se encontraron boletas."); //si no se logra encontrar, se retorna un mensaje de no encontrado
                }
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}