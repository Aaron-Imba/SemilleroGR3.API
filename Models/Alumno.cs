namespace SemilleroGR3.API.Models
{
    public class Alumno
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public bool Activo { get; set; }
    }
}
