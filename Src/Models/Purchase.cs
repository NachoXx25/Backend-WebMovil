namespace taller1WebMovil.Src.Models
{
    public class Purchase
    {
        public int Id { get; set; } //Clave primaria

        public int Quantity { get; set; } //Requerido

        public int Total { get; set; } //Requerido

        public DateTime Date { get; set; } //Requerido


        //Relaciones
        public User User { get; set; } = null!; //Relacion con la tabla User
        public int UserId { get; set; } //Clave foranea
    
        public Product Product { get; set; } = null!; //Relacion con la tabla Product
        public int ProductId { get; set; } //Clave foranea

        public int UnitPrice { get; set; } //Requerido

        public string ProductName { get; set; } = string.Empty; //Requerido

        public string ProductType { get; set; } = string.Empty; //Requerido
        
    }
}
