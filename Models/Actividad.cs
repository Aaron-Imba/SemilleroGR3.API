namespace SemilleroGR3.API.Models
{
    public class Actividad
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool EsParaCasa { get; set; }
        public bool Activo { get; set; }
    }
}
