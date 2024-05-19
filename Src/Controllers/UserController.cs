using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _service; //se inyecta el servicio de cuentas
    
        private readonly IMapperService _mapper; //se inyecta el servicio de mapeo
        public UserController(IAccountService service, IMapperService mapper) //se inyecta el servicio de cuentas y el servicio de mapeo
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
                var users = await _service.SearchUsers(searchString); //se llama al metodo de buscar usuarios
                if (!users.Any())
                {
                    return NotFound("No se encontraron usuarios que coincidan con la búsqueda."); //si no se logra encontrar, se retorna un mensaje de no encontrado
                }
                return Ok(users); //se retornan los usuarios
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); //si no se logra encontrar, se retorna un mensaje de error
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{rut}/enable")]
        public ActionResult<UserDTO> EnableAccount(string Rut){
            try{
                var user = _service.EnableAccount(Rut).Result; //se llama al metodo de habilitar cuenta
                return Ok(_mapper.UserToUserDTO(user)); //se retorna el usuario

            }catch(Exception e){
                return BadRequest(e.Message); //si no se logra habilitar, se retorna un mensaje de error
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{rut}/disable")]
        public ActionResult<UserDTO> DisableAccount(string Rut){
            try{
                var user = _service.DisableAccount(Rut).Result; //se llama al metodo de deshabilitar cuenta
                return Ok(_mapper.UserToUserDTO(user)); //se retorna el usuario

            }catch(Exception e){
                return BadRequest(e.Message); //si no se logra deshabilitar, se retorna un mensaje de error
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //si el modelo no es valido, se retorna un mensaje de error
            }

            
            bool editResult = await _service.EditUser(IdUser, userProfileEdit); //se llama al metodo de editar usuario

            if (editResult)
            {
                return Ok("Usuario editado con éxito"); //si se logra editar, se retorna un mensaje de éxito
            }
            else
            {
                return NotFound("Usuario no encontrado"); //si no se logra editar, se retorna un mensaje de no encontrado
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
                return BadRequest(ModelState); //si el modelo no es valido, se retorna un mensaje de error
            }

            bool editPassword = await _service.EditPassword(IdUser, editPasswordDTO); //se llama al metodo de editar contraseña

            if(editPassword)
            {
                return Ok("Contraseña cambiada con exito"); //si se logra cambiar, se retorna un mensaje de éxito
            }

            else
            {
                return NotFound("No se ha podido generar el cambio de contraseña");  //si no se logra cambiar, se retorna un mensaje de no encontrado
            }
        }
    }
}