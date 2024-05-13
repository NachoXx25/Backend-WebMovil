using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MovilController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        private readonly IPurchaseService _purchaseService;

        public MovilController(IUserRepository userRepository, IAuthService authService, IPurchaseService purchaseService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _purchaseService = purchaseService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUserDTO loginUserDTO)
        {
            var userClient = await _userRepository.GetUserByEmail(loginUserDTO.Email);

            if (userClient != null)
            {
                if (userClient.Role != null && userClient.Role.Name != "User")
                {
                    return BadRequest("Solo se permite el uso a clientes");
                }

                var result = await _authService.LoginUser(loginUserDTO);

                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Credenciales inválidas.");
            }
            return BadRequest("Usuario no encontrado.");
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
                var tickets = await _purchaseService.SearchTicket(userId);
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}