using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class NameAttributeEditProfile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string nombre = value as string;
            if (!string.IsNullOrEmpty(nombre))
            {
                if (nombre.Length < 10)
                {
                    return new ValidationResult("El nombre debe tener al menos 10 caracteres");
                }
                if (nombre.Length > 64)
                {
                    return new ValidationResult("El nombre debe tener como máximo 64 caracteres");
                }
            }
            return ValidationResult.Success;
        }
    }
}