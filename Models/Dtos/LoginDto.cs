using System.ComponentModel.DataAnnotations;

namespace DozoWeb.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario o correo electrónico es obligatorio.")]
        public required string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public required string Password { get; set; }
    }
}