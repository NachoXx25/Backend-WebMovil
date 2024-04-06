namespace taller1WebMovil.Src.Models
{
    public class Product
    {
        public int Id { get; set; } //Primary Key

        public string Name { get; set; } = string.Empty; //Required

        public string Type { get; set; } = string.Empty; //Required

        public int Stock { get; set; } //Required

        public int Price { get; set; } //Required
        public string Image { get; set; } = string.Empty; //Required
    }
}