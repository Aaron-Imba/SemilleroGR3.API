namespace SemilleroGR3.API.Models
{
    public class TareaCasa_Seguimiento
    {
        public int Id { get; set; }
        public int ActividadId { get; set; }
        public int AlumnoId { get; set; }
        public int FamiliaId { get; set; }
        public bool Realizada { get; set; }
        public string? ComentarioBreve { get; set; }
        public DateTime? FechaReporte { get; set; }

        // Navegación para traer el título y descripción de la actividad
        public Actividad? Actividad { get; set; }
    }
}
