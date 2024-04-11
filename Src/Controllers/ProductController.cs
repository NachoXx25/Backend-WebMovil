using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace taller1WebMovil.Src.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        
    }
}