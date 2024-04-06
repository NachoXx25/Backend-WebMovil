using System.ComponentModel.DataAnnotations;
using taller1WebMovil.Src.Validations;

namespace taller1WebMovil.Src.DTOs
{
    public class RegisterUserDTO
    {
        [Rut (ErrorMessage = "El RUT no es válido")]
        [Required (ErrorMessage = "El RUT es requerido")]
        public required string Rut { get; set; }

        [Required (ErrorMessage = "El nombre es requerido")]
        [MinLength (8, ErrorMessage = "El nombre debe tener al menos 8 caracteres")]
        [MaxLength (255, ErrorMessage = "El nombre debe tener como máximo 255 caracteres")]

        public string Name { get; set; } = string.Empty;
        [Required (ErrorMessage = "La fecha de nacimiento es requerida")]

        public DateTime BirthDate { get; set; }
        [Required (ErrorMessage = "El email es requerido")]
        [EmailAddress (ErrorMessage = "El email no es válido")]

        public string Email { get; set; } = string.Empty;
        [Required (ErrorMessage = "El género es requerido")]

        public string Gender { get; set; } = string.Empty;
        [Required (ErrorMessage = "La contraseña es requerida")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]+$", ErrorMessage = "La Contraseña debe ser alfanumérica.")]
        [MinLength (8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [MaxLength (20, ErrorMessage = "La contraseña debe tener como máximo 20 caracteres")]
        public string Password { get; set; } = string.Empty;


        [Required (ErrorMessage = "La confirmación de la contraseña es requerida")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}