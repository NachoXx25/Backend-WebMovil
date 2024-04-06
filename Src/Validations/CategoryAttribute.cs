using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class CategoryAttribute : ValidationAttribute
    {
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var categorias = new string[] {"Tecnología", "Electrohogar", "Juguetería", "Ropa", "Muebles", "Comida", "Libros"};
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Categoría es requerida");
            }
            if (!categorias.Contains(value.ToString()))
            {
                return new ValidationResult("Categoría no válida (categorías válidas: Tecnología, Electrohogar, Juguetería, Ropa, Muebles, Comida, Libros)");
            }
            return ValidationResult.Success;
        }   
    }
}