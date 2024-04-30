using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using taller1WebMovil.Src.Validations;

namespace taller1WebMovil.Src.DTOs
{
    public class UserProfileEditDTO
    {
        [Required(ErrorMessage = "El campo nombre es requerido")] //nombre requerido
        [MinLength(10, ErrorMessage = "El nombre debe tener al menos 10 caracteres")] //nombre minimo de 10 caracteres
        [MaxLength(64, ErrorMessage = "El nombre debe tener como máximo 64 caracteres")] //nombre maximo de 64 caracteres
        public string Name { get; set; } = string.Empty;// Nombre del usuario
        [DataType(DataType.Date)] //fecha de nacimiento
        [BirthDate] //validacion de fecha de nacimiento, previene que no sea nula y que sea una fecha valida antes que hoy
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //formato de fecha
        public DateTime BirthDate { get; set; } // Fecha de nacimiento del usuario

        [Gender]
        public string Gender { get; set; } = string.Empty;// Género del usuario
    }
}