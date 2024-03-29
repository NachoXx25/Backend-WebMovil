using Bogus.DataSets;

namespace taller1WebMovil.Src.Models
{
    public class User
    {
        public required string Rut { get; set; }

        public string Name { get; set; } = string.Empty;

        public Date Birth_date { get; set; } = new Date();

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        //Relationships
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}