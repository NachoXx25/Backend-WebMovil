using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace taller1WebMovil.Src.Validations
{
    public class NameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) //Validación de nombre
    {
       
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult("El nombre es requerido"); //Rut requerido
        }
        
        string? name = value.ToString();

        if (name.Length < 8)
        {
            return new ValidationResult("El nombre debe tener al menos 8 caracteres"); //Nombre debe tener al menos 8 caracteres
        }
        if (name.Length > 255)
        {
            return new ValidationResult("El nombre debe tener como máximo 255 caracteres"); //Nombre debe tener como máximo 255 caracteres
        }
        Regex regex = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]*$");

        if (!regex.IsMatch(name))
        {
            return new ValidationResult("El nombre solo puede contener letras"); //Nombre solo puede contener letras
        }
        return ValidationResult.Success;
    }
    }
}