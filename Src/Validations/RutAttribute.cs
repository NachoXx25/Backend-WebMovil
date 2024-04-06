using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations;

    public class RutAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) //Validación de Rut
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult("Rut is required"); //Rut requerido
        }

        string? rut = value.ToString();

        if (!IsValidRut(rut))
        {
            return new ValidationResult("Invalid Rut"); //Rut inválido
        }

        return ValidationResult.Success;
    }

    private static bool IsValidRut(string? rut) //Validación de Rut
    {
        if (string.IsNullOrEmpty(rut))
        {
            return false;
        }

        rut = rut.Replace(".", "").Replace("-", "").ToLower();
        if (!int.TryParse(rut.AsSpan(0, rut.Length - 1), out int number)) 
        {
            return false; 
        }

        char dv = rut[^1]; //Digito verificador
        return dv == GetDV(number); 
    }

    private static char GetDV(int number) //Obtener digito verificador
    {
        int m = 0,
            s = 1;
        for (; number != 0; number /= 10) //Ciclo para obtener el digito verificador
        {
            s = (s + number % 10 * (9 - m++ % 6)) % 11; //Calculo del digito verificador
        }

        return (char)(s != 0 ? s + 47 : 75); //Retorno del digito verificador
    }
    }
