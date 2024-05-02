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
        //nombre requerido
        [NameAttributeEditProfile]
        public string Name { get; set; } = string.Empty;// Nombre del usuario
        [DataType(DataType.Date)] //fecha de nacimiento
        [BirthDate] //validacion de fecha de nacimiento, previene que no sea nula y que sea una fecha valida antes que hoy
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //formato de fecha
        public DateTime BirthDate { get; set; } // Fecha de nacimiento del usuario

        [GenderAttributeEditProfile]
        public string Gender { get; set; } = string.Empty;// Género del usuario
    }
}