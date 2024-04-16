namespace taller1WebMovil.Src.DTOs
{
    public class UserDTO
    {
        public required string Rut { get; set; } //Rut del usuario

        public string Name { get; set; } = string.Empty; //Nombre del usuario

        public DateTime BirthDate { get; set; } //Fecha de nacimiento del usuario

        public string Email { get; set; } = string.Empty; //Email del usuario

        public string Gender { get; set; } = string.Empty; //Genero del usuario

        public bool Active { get; set; } // Required
    }
}