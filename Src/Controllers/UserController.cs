using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _service; //inyeccion de dependencias
    
        private readonly IMapperService _mapper; //inyeccion de dependencias
        public UserController(IAccountService service, IMapperService mapper) //inyeccion de dependencias
        {
            _service = service;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search/{searchString?}")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> SearchUser(string searchString = null)
        {
            try
            {
                var users = await _service.SearchUsers(searchString);
                if (!users.Any())
                {
                    return NotFound("No se encontraron usuarios que coincidan con la búsqueda.");
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{rut}/enable")]
        public ActionResult<UserDTO> EnableAccount(string Rut){
            try{
                var user = _service.EnableAccount(Rut).Result;
                return Ok(_mapper.UserToUserDTO(user));

            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{rut}/disable")]
        public ActionResult<UserDTO> DisableAccount(string Rut){
            try{
                var user = _service.DisableAccount(Rut).Result;
                return Ok(_mapper.UserToUserDTO(user));

            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPut("edit-profile")]
        public async Task<ActionResult<string>> EditAccount([FromBody] UserProfileEditDTO userProfileEdit)
        {

            var token = Request.Headers["Authorization"].ToString().Split(' ')[1]; // Se obtiene el token
            var handler = new JwtSecurityTokenHandler(); // Se crea un manejador de tokens
            var jwtToken = handler.ReadJwtToken(token); // Se lee la token
            // Se accede a los claims de la token
            var userId = jwtToken.Claims.First(claim => claim.Type == "Id").Value;
            int IdUser = int.Parse(userId); // Se obtiene el id del usuario y se parsea a entero
            // Verificar si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Llamar al método de servicio para editar el usuario
            bool editResult = await _service.EditUser(IdUser, userProfileEdit);

            // Verificar si la edición fue exitosa
            if (editResult)
            {
                // Devolver una respuesta exitosa
                return Ok("Usuario editado con éxito");
            }
            else
            {
                // Manejar el caso en que la edición no fue exitosa
                return NotFound("Usuario no encontrado"); 
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("edit-password")]
        public async Task<ActionResult<string>> EditPasswordUser([FromBody] EditPasswordDTO editPasswordDTO)
        {
            var token = Request.Headers["Authorization"].ToString().Split(' ')[1]; // Se obtiene el token
            var handler = new JwtSecurityTokenHandler(); // Se crea un manejador de tokens
            var jwtToken = handler.ReadJwtToken(token); // Se lee la token
            // Se accede a los claims de la token
            var userId = jwtToken.Claims.First(claim => claim.Type == "Id").Value;
            int IdUser = int.Parse(userId); // Se obtiene el id del usuario y se parsea a entero
            Console.WriteLine($"Mod y proceso");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool editPassword = await _service.EditPassword(IdUser, editPasswordDTO);

            if(editPassword)
            {
                return Ok("Contraseña cambiada con exito");
            }

            else
            {
                return NotFound("No se ha podido generar el cambio de contraseña"); 
            }
        }
    }
}