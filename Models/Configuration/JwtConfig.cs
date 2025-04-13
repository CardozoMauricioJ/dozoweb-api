namespace DozoWeb.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenLifetimeInMinutes { get; set; } = 60; // Duraci�n predeterminada del token
    }
}