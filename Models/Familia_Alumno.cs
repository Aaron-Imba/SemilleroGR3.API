namespace SemilleroGR3.API.Models
{
    public class Familia_Alumno
    {
        public int FamiliaId { get; set; }
        public int AlumnoId { get; set; }
        public bool EsRepresentantePrincipal { get; set; }

        // Navegación para traer los datos del hijo directamente
        public Alumno? Alumno { get; set; }
    }
}
