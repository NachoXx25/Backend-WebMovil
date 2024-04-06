using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class GenderAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var generos = new string[] { "Masculino", "Femenino", "Otro", "Prefiero no decirlo" };
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Género es requerido");
            }
            if (!generos.Contains(value.ToString()))
            {
                return new ValidationResult("Género no válido (generos válidos: Masculino, Femenino, Otro, Prefiero no decirlo)");
            }
            return ValidationResult.Success;
        }   
    }
}