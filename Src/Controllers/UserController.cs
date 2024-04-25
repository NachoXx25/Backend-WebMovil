using System;
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers() //metodo para obtener todos los usuarios
        {
            var users = _service.GetNonAdminUsers().Result; //se obtienen todos los usuarios
            var result = users.Select(user => _mapper.UserToUserDTO(user)).ToList(); //se mapean los usuarios
            if (result.Count == 0) //si no hay usuarios
            {
                return NotFound("No se encontraron usuarios registrados"); //se retorna not found
            }
            return Ok(result); //se retornan los usuarios
        }

        [HttpPut("{rut}/disable")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDTO> DisableAccount(string rut){
            try{
                var user = _service.DisableAccount(rut).Result;
                return Ok(_mapper.UserToUserDTO(user));

            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{rut}/enable")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDTO> EnableAccount(string Rut){
            try{
                var user = _service.EnableAccount(Rut).Result;
                return Ok(_mapper.UserToUserDTO(user));

            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("edit-profile/{userId}")]
        public async Task<ActionResult<string>> EditAccount(int userId, [FromBody] UserProfileEditDTO userProfileEdit)
        {
            // Verificar si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Llamar al método de servicio para editar el usuario
            bool editResult = await _service.EditUser(userId, userProfileEdit);

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
    }
}