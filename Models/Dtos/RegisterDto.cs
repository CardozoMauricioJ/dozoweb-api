using System.ComponentModel.DataAnnotations;

namespace DozoWeb.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "El correo electr�nico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electr�nico no es v�lido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contrase�a es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contrase�a debe tener al menos 6 caracteres.", MinimumLength = 6)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public required string Rol { get; set; } // "UsuarioComun" o "Cliente"
    }
}