using System.ComponentModel.DataAnnotations;
using taller1WebMovil.Src.Validations;

namespace taller1WebMovil.Src.DTOs
{
    public class RegisterUserDTO
    {
        [Rut] // el rut es unico?
        [Required (ErrorMessage = "El RUT es requerido")]
        public required string Rut { get; set; }

        [Required (ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre solo puede contener letras")]
        [MinLength (8, ErrorMessage = "El nombre debe tener al menos 8 caracteres")]
        [MaxLength (255, ErrorMessage = "El nombre debe tener como máximo 255 caracteres")]
        public string Name { get; set; } = string.Empty;


        [DataType(DataType.Date)]
        [BirthDate]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        [Required (ErrorMessage = "El email es requerido")]
        [EmailAddress (ErrorMessage = "El email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Gender]
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