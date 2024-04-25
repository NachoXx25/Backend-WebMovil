using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1WebMovil.Src.DTOs
{
    public class EditPasswordDTO
    {
        public string NewPassword { get; set; } = string.Empty;// Nombre del usuario
        public string Password { get; set; } = string.Empty;// Género del usuario
    }
}