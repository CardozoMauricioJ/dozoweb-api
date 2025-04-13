using System.ComponentModel.DataAnnotations;

namespace DozoWeb.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.", MinimumLength = 6)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public required string Rol { get; set; } // "UsuarioComun" o "Cliente"
    }
}