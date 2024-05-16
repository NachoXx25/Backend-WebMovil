using System.ComponentModel.DataAnnotations;


namespace taller1WebMovil.Src.Validations
{
    public class BirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var birthDate = (DateTime)value; //Se obtiene la fecha de cumpleaños
                if (birthDate > DateTime.Now) //Si la fecha de cumpleaños es mayor a la fecha actual
                {
                    return new ValidationResult("La fecha de cumpleaños no puede ser mayor a la fecha actual."); //Validación fallida
                }
                return ValidationResult.Success; //Validación exitosa
            }
            return new ValidationResult("La fecha de cumpleaños es requerida."); //Validación fallida
        }
    }
}