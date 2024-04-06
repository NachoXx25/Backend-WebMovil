using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.DTOs
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "El campo nombre es requerido")] //nombre requerido
        [MinLength(10, ErrorMessage = "El nombre debe tener al menos 10 caracteres")] //nombre minimo de 10 caracteres
        [MaxLength(64, ErrorMessage = "El nombre debe tener como máximo 64 caracteres")] //nombre maximo de 64 caracteres
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre solo puede contener letras")] //nombre solo letras
        public string Name { get; set; } = string.Empty;

        
        [Category] //validacion de categoria, previene que no sea nula y que sea una categoria valida (las especificadas en el taller)
        public string Type { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El campo precio es requerido")] //precio requerido
        [Range(0, 99999999, ErrorMessage = "El stock debe ser mayor o igual a 0 y menor de 100000000")] //precio entre 0 y 99999999
        public int Price { get; set; }


        [Required(ErrorMessage = "El campo stock es requerido")] //stock requerido
        [Range(0, 99999, ErrorMessage = "El stock debe ser mayor o igual a 0 y menor de 100000")] //stock entre 0 y 99999
        public int Stock { get; set; }

        //validaciones del formato de la imagen y su peso pendientes
        [Required(ErrorMessage = "El campo imagen es requerido")] //imagen requerida
        public string Image { get; set; } = string.Empty;

    }
}