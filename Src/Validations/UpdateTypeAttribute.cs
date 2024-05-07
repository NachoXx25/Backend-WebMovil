using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.Validations
{
    public class UpdateTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) //Validación de categoría
        {
            var categorias = new string[] {"Tecnología", "Electrohogar", "Juguetería", "Ropa", "Muebles", "Comida", "Libros"};
            if (!categorias.Any(categoria => categoria.Equals(value.ToString(), StringComparison.Ordinal)))
            {
                return new ValidationResult("Categoría no válida (categorías válidas: Tecnología, Electrohogar, Juguetería, Ropa, Muebles, Comida, Libros)"); //Categoría no válida
            }
            return ValidationResult.Success; //Categoría válida
        }   
    }
}