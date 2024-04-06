using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;

namespace taller1WebMovil.Src.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _service; //inyeccion de dependencias

        public UserController(IUserRepository service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers() //metodo para obtener todos los usuarios
        {
            var result = _service.GetUsers().Result; //se obtienen todos los usuarios
            return Ok(result); //se retornan los usuarios
        }
    }
}