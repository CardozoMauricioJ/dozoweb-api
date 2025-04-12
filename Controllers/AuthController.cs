using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DozoWeb.Models;

namespace DozoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Aquí irán los métodos para registrar e iniciar sesión
    }
}