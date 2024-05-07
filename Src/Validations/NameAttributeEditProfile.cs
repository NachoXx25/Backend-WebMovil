using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace taller1WebMovil.Src.Validations
{
    public class NameAttributeEditProfile : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            string? name = value as string;
            if (!string.IsNullOrEmpty(name))
            {
                if (name.Length < 10)
                {
                    return new ValidationResult("El nombre debe tener al menos 10 caracteres");
                }
                if (name.Length > 64)
                {
                    return new ValidationResult("El nombre debe tener como máximo 64 caracteres");
                }
                Regex regex = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]*$");

                if (!regex.IsMatch(name))
                {
                    return new ValidationResult("El nombre solo puede contener letras"); //Nombre solo puede contener letras
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}