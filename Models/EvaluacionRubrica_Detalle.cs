using System.ComponentModel.DataAnnotations.Schema;

namespace SemilleroGR3.API.Models
{
    public class EvaluacionRubrica_Detalle
    {
        public int Id { get; set; }

        // El nombre real en tu base de datos es EvaluacionId
        // Pero Entity Framework estaba buscando EvaluacionRubricaId
        [Column("EvaluacionId")]
        public int EvaluacionId { get; set; }

        // Navegación a la evaluación padre. Indicamos explícitamente que la FK es EvaluacionId
        [ForeignKey("EvaluacionId")]
        public EvaluacionRubrica? EvaluacionRubrica { get; set; }

        public int CriterioId { get; set; }
        public int NivelId { get; set; }
        public string? ObservacionEspecifica { get; set; }

        // Navegación para obtener los nombres dinámicos
        [ForeignKey("CriterioId")]
        public Rubrica_Criterio? Criterio { get; set; }

        [ForeignKey("NivelId")]
        public Rubrica_Nivel? Nivel { get; set; }
    }
}