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
    public class MovilController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public MovilController(IUserRepository userRepository, IAuthService authService)
        {
            _authService = authService;
            _userRepository = userRepository;
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
    }
}