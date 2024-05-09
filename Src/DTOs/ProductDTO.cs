using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using taller1WebMovil.Src.Validations;

namespace taller1WebMovil.Src.DTOs
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "El campo nombre es requerido")] 
        [MinLength(10, ErrorMessage = "El nombre debe tener al menos 10 caracteres")] //nombre minimo de 10 caracteres
        [MaxLength(64, ErrorMessage = "El nombre debe tener como máximo 64 caracteres")] //nombre maximo de 64 caracteres
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ0-9\s]*$", ErrorMessage = "El nombre debe estar en alfabeto español y puede contener números")]
        public string Name { get; set; } = string.Empty;

        [Type]
        public string Type { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El campo precio es requerido")]
        [Range(1, 99999999, ErrorMessage = "El precio debe ser mayor o igual a 0 y menor de 100000000")] //precio entre 0 y 99999999
        public int Price { get; set; }


        [Required(ErrorMessage = "El campo stock es requerido")] 
        [Range(0, 99999, ErrorMessage = "El stock debe ser mayor o igual a 0 y menor de 100000")] //stock entre 0 y 99999
        public int Stock { get; set; }

        public string Image { get; set; } = string.Empty;

    }
}