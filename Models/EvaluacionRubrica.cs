using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SemilleroGR3.API.Models
{
    public class EvaluacionRubrica
    {
        public int Id { get; set; }
        public int RubricaId { get; set; }
        public int DocenteId { get; set; }
        public int AlumnoId { get; set; }
        public int? UnidadId { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public string? NotaGeneral { get; set; }
        public bool EsPublicada { get; set; }

        // Navegación para traer todo el detalle de una vez
        [InverseProperty("EvaluacionRubrica")]
        public ICollection<EvaluacionRubrica_Detalle> Detalles { get; set; } = new List<EvaluacionRubrica_Detalle>();
    }
}
