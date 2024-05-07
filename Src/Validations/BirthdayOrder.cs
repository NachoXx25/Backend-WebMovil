using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace taller1WebMovil.Src.Validations
{
    public class DateOrderAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success; // Permitir valores nulos, otra validación se encargará de eso
            }

            // Intentar analizar la fecha en diferentes formatos
            string[] formats = { 
                "yyyy-MM-dd", 
                "MM-dd-yyyy", 
                "dd-MM-yyyy", 
                "yyyy/MM/dd", 
                "MM/dd/yyyy", 
                "dd/MM/yyyy", 
                "yyyy.MM.dd", 
                "MM.dd.yyyy", 
                "dd.MM.yyyy", 
                "yyyyMMdd", 
                "MMddyyyy", 
                "ddMMyyyy" 
            };
            DateTime parsedDate;
            if (!DateTime.TryParseExact(value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return new ValidationResult("Formato de fecha no válido.");
            }

            // Normalizar la fecha al formato "yyyy-MM-dd"
            string normalizedDate = parsedDate.ToString("yyyy-MM-dd");
            
            // Asignar la fecha normalizada de vuelta al valor
            validationContext.ObjectType.GetProperty(validationContext.MemberName)?.SetValue(validationContext.ObjectInstance, normalizedDate);

            return ValidationResult.Success;
        }
    }
}