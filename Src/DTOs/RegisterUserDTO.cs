using System.ComponentModel.DataAnnotations;
using taller1WebMovil.Src.Validations;

namespace taller1WebMovil.Src.DTOs
{
    public class RegisterUserDTO
    {
        [Rut] 
        [Required (ErrorMessage = "El RUT es requerido")] //rut requerido
        public required string Rut { get; set; }

        [Name] //validacion de nombre, previene que no sea nulo y que sea un nombre valido
        public string Name { get; set; } = string.Empty;


        [DataType(DataType.Date)] //fecha de nacimiento
        [BirthDate] //validacion de fecha de nacimiento, previene que no sea nula y que sea una fecha valida antes que hoy
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //formato de fecha
        public DateTime BirthDate { get; set; }


        [Required (ErrorMessage = "El email es requerido")] //email requerido
        [EmailAddress (ErrorMessage = "El email no es válido")] //email valido
        public string Email { get; set; } = string.Empty;

        [Gender] //validacion de genero, previene que no sea nulo y que sea un genero valido (los especificados en el taller)
        public string Gender { get; set; } = string.Empty;

        [Required (ErrorMessage = "La contraseña es requerida")] //contraseña requerida
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]+$", ErrorMessage = "La Contraseña debe ser alfanumérica.")] //contraseña alfanumerica
        [MinLength (8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")] //contraseña minimo de 8 caracteres
        [MaxLength (20, ErrorMessage = "La contraseña debe tener como máximo 20 caracteres")] //contraseña maximo de 20 caracteres
        public string Password { get; set; } = string.Empty;


        [Required (ErrorMessage = "La confirmación de la contraseña es requerida")] //confirmacion de contraseña requerida
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")] //comparacion de contraseñas
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}