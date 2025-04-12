using Microsoft.AspNetCore.Identity;

namespace DozoWeb.Models
{
    public class AppUser : IdentityUser
    {
        // Puedes agregar propiedades adicionales aqu�, como NombreCompleto, etc.
        public string? Rol { get; set; } // Campo para almacenar el rol del usuario
    }
}