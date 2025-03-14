using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DozoWeb.Models
{
    public class Cerveceria
    {
        public int Id { get; set; } // Clave primaria

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } // Nombre de la cervecería

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion { get; set; } // Dirección del lugar

        [Range(0, 1000, ErrorMessage = "El precio promedio debe estar entre 0 y 1000.")]
        public decimal PrecioPromedio { get; set; } // Precio promedio

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public double Latitud { get; set; } = 0.0; // Coordenadas de ubicación

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public double Longitud { get; set; } = 0.0; // Coordenadas de ubicación
        public virtual ICollection<Opinion>? Opiniones { get; set; } //= new List<Opinion>();

    }

}
