using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.DTOs
{
    public class EditPasswordDTO
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")] //se valida que la contraseña actual sea requerida
        public string Password { get; set; } = string.Empty; 

        [Required(ErrorMessage = "La nueva contraseña es requerida")] //se valida que la nueva contraseña sea requerida
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]+$", ErrorMessage = "La Contraseña debe ser alfanumérica.")] //se valida que la nueva contraseña sea alfanumérica
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")] //se valida que la nueva contraseña tenga al menos 8 caracteres
        [MaxLength(20, ErrorMessage = "La contraseña debe tener como máximo 20 caracteres")] //se valida que la nueva contraseña tenga como máximo 20 caracteres
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de la nueva contraseña es requerida")] //se valida que la confirmación de la nueva contraseña sea requerida
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")] //se valida que la confirmación de la nueva contraseña sea igual a la nueva contraseña
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}