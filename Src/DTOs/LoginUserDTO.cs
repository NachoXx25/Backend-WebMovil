using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.DTOs
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "El campo email es requerido")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo contraseña es requerido")]
        public string Password { get; set; } = string.Empty;
    }
}