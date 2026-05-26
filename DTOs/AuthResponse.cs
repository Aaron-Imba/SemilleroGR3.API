namespace SemilleroGR3.API.DTOs
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
