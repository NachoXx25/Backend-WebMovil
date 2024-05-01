using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class GenderAttributeEditProfile : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Verificar si el valor del género es nulo o vacío
            if (string.IsNullOrEmpty((string)value))
            {
                return ValidationResult.Success; // El género está vacío o nulo
            }

            var generos = new string[] { "Masculino", "Femenino", "Otro", "Prefiero no decirlo" }; // Géneros válidos
            if (!generos.Contains(value.ToString()))
            {
                return new ValidationResult("Género no válido (géneros válidos: Masculino, Femenino, Otro, Prefiero no decirlo)"); // Género no válido
            }
            return ValidationResult.Success; // Género válido
        }   
    }
}