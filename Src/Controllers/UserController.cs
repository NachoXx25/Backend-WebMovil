using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _service; //inyeccion de dependencias
    
        private readonly IMapperService _mapper; //inyeccion de dependencias
        public UserController(IUserRepository service, IMapperService mapper) //inyeccion de dependencias
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers() //metodo para obtener todos los usuarios
        {
            var users = _service.GetUsers().Result; //se obtienen todos los usuarios
            var result = users.Select(user => _mapper.UserToUserDTO(user)).ToList(); //se mapean los usuarios
            if (result.Count == 0) //si no hay usuarios
            {
                return NotFound("No se encontraron usuarios registrados"); //se retorna not found
            }
            return Ok(result); //se retornan los usuarios
        }
    }
}