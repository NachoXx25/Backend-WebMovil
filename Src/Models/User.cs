using Bogus.DataSets;

namespace taller1WebMovil.Src.Models
{
    public class User
    {
        public int Id { get; set; } //Primary Key
        public required string Rut { get; set; } //Required

        public string Name { get; set; } = string.Empty; //Required

        public DateTime BirthDate { get; set; } //Required

        public string Email { get; set; } = string.Empty; //Required

        public string Password { get; set; } = string.Empty; //Required

        public string Gender { get; set; } = string.Empty; //Required   

        public bool Active { get; set; } // Required

        //Relationships
        public int RoleId { get; set; } //Foreign Key
        public Role Role { get; set; } = null!; //Navigation Property
    }
}