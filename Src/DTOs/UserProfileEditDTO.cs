using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1WebMovil.Src.DTOs
{
    public class UserProfileEditDTO
    {
        public string Name { get; set; } = string.Empty;// Nombre del usuario
        public DateTime BirthDate { get; set; } // Fecha de nacimiento del usuario
        public string Gender { get; set; } = string.Empty;// Género del usuario
    }
}