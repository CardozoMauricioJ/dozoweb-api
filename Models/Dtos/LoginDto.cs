using System.ComponentModel.DataAnnotations;

namespace DozoWeb.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario o correo electr�nico es obligatorio.")]
        public required string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "La contrase�a es obligatoria.")]
        public required string Password { get; set; }
    }
}