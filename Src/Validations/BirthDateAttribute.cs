using System.ComponentModel.DataAnnotations;


namespace taller1WebMovil.Src.Validations
{
    public class BirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var birthDate = (DateTime)value;
            if (birthDate > DateTime.Now)
            {
                return new ValidationResult("La fecha de cumpleaños no puede ser mayor a la fecha actual.");
            }
            return ValidationResult.Success;
        }
    }
}