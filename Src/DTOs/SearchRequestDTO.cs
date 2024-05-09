using System.ComponentModel.DataAnnotations;

namespace taller1WebMovil.Src.DTOs
{
    public class SearchRequest
    { //email requerido 
        public string SearchString { get; set; } = string.Empty;  
    }
}