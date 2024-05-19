namespace taller1WebMovil.Src.DTOs
{
    public class PurchaseDTO
    {
        public int Id { get; set; } //Clave primaria

        public int ProductId { get; set; } //Clave foranea

        public string ProductName { get; set; } = string.Empty; 

        public string ProductType { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int Total { get; set; }

        public DateTime Date { get; set; } 

    }
}