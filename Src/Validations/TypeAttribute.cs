using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class TypeAttribute : ValidationAttribute
    {
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) //Validación de categoría
        {
            var categorias = new string[] {"Tecnología", "Electrohogar", "Juguetería", "Ropa", "Muebles", "Comida", "Libros"};
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Categoría es requerida"); //Categoría requerida
            }
            if (!categorias.Any(categoria => categoria.Equals(value.ToString(), StringComparison.Ordinal)))
            {
                return new ValidationResult("Categoría no válida (categorías válidas: Tecnología, Electrohogar, Juguetería, Ropa, Muebles, Comida, Libros)"); //Categoría no válida
            }
            return ValidationResult.Success; //Categoría válida
        }   
    }
}