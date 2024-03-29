namespace taller1WebMovil.Src.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int Price { get; set; } 

        public int Stock { get; set; }

        public string image { get; set; } = string.Empty;
    }
}