namespace SemilleroGR3.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
    }
}
