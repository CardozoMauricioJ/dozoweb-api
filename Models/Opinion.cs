using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DozoWeb.Models
{
    public class Opinion
    {
        public int Id { get; set; }
        [Required]
        public string Usuario { get; set; } // Nombre del usuario que deja la opinión
        [Range(1, 5, ErrorMessage = "El puntaje debe estar entre 1 y 5.")]
        public int Puntaje { get; set; }
        public string Comentario { get; set; } // Comentario opcional
        public DateTime Fecha { get; set; } // Fecha de la opinión

        // Relación con Cerveceria
        [Required]
        public int CerveceriaId { get; set; }
        [JsonIgnore]
        public virtual Cerveceria? Cerveceria { get; set; }
    }
}
