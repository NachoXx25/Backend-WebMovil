namespace taller1WebMovil.Src.Models
{
    public class BlacklistedToken
    {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    }
}