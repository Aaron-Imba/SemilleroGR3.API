using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemilleroGR3.API.Data;
using SemilleroGR3.API.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace SemilleroGR3.API.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class FamiliaController : ControllerBase
    {
        private readonly SemilleroContext _context;

        public FamiliaController(SemilleroContext context)
        {
            _context = context;
        }

        // MAUI Endpoint: familia/{familiaId}/hijos
        [HttpGet("familia/{familiaId}/hijos")]
        public async Task<IActionResult> GetHijos(int familiaId)
        {
            var hijos = await _context.Familia_Alumno
                .Where(fa => fa.FamiliaId == familiaId && fa.Alumno != null && fa.Alumno.Activo)
                .Select(fa => fa.Alumno)
                .ToListAsync();

            return Ok(hijos);
        }

        // MAUI Endpoint: evaluaciones/alumno/{alumnoId}
        [HttpGet("evaluaciones/alumno/{alumnoId}")]
        public async Task<IActionResult> GetEvaluaciones(int alumnoId)
        {
            var evaluaciones = await _context.EvaluacionRubrica
                .Where(e => e.AlumnoId == alumnoId && e.EsPublicada)
                .Include(e => e.Detalles).ThenInclude(d => d.Criterio)
                .Include(e => e.Detalles).ThenInclude(d => d.Nivel)
                .OrderByDescending(e => e.FechaEvaluacion)
                .Select(e => new
                {
                    e.Id,
                    e.RubricaId,
                    e.DocenteId,
                    e.AlumnoId,
                    e.UnidadId,
                    e.FechaEvaluacion,
                    e.NotaGeneral,
                    // Aplanamos el detalle para MAUI
                    Detalles = e.Detalles.Select(d => new
                    {
                        d.Id,
                        d.CriterioId,
                        CriterioNombre = d.Criterio != null ? d.Criterio.Nombre : "Sin Criterio",
                        d.NivelId,
                        NivelNombre = d.Nivel != null ? d.Nivel.Nombre : "Sin Nivel",
                        d.ObservacionEspecifica
                    }).ToList()
                })
                .ToListAsync();

            return Ok(evaluaciones);
        }

        // MAUI Endpoint: tareas/alumno/{alumnoId}
        [HttpGet("tareas/alumno/{alumnoId}")]
        public async Task<IActionResult> GetTareasCasa(int alumnoId)
        {
            var tareas = await _context.TareaCasa_Seguimiento
                .Where(t => t.AlumnoId == alumnoId && t.Actividad != null && t.Actividad.Activo)
                .Select(t => new
                {
                    t.Id,
                    t.ActividadId,
                    TituloActividad = t.Actividad!.Titulo,
                    DescripcionActividad = t.Actividad.Descripcion,
                    t.AlumnoId,
                    t.Realizada,
                    t.ComentarioBreve,
                    t.FechaReporte
                })
                .ToListAsync();

            return Ok(tareas);
        }

        // MAUI Endpoint: tareas/{tareaId}/estado
        [HttpPut("tareas/{tareaId}/estado")]
        public async Task<IActionResult> ActualizarEstadoTarea(int tareaId, [FromBody] TareaUpdateDto dto)
        {
            var tarea = await _context.TareaCasa_Seguimiento.FindAsync(tareaId);

            if (tarea == null) return NotFound();

            tarea.Realizada = dto.Realizada;
            tarea.ComentarioBreve = dto.ComentarioBreve;
            tarea.FechaReporte = dto.FechaReporte;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Tarea actualizada correctamente" });
        }

        // MAUI Endpoint: familia/progreso/{alumnoId}
        [HttpGet("familia/progreso/{alumnoId}")]
        public async Task<IActionResult> GetProgresoHijo(int alumnoId)
        {
            // 1. Buscamos solo la última evaluación publicada del alumno
            var ultimaEvaluacion = await _context.EvaluacionRubrica
                .Include(e => e.Detalles).ThenInclude(d => d.Criterio)
                .Include(e => e.Detalles).ThenInclude(d => d.Nivel)
                .Where(e => e.AlumnoId == alumnoId && e.EsPublicada)
                .OrderByDescending(e => e.FechaEvaluacion)
                .FirstOrDefaultAsync();

            // 2. Si no hay evaluaciones, devolvemos una lista vacía
            if (ultimaEvaluacion == null)
            {
                return Ok(new List<object>());
            }

            // 3. Aplanamos el detalle con los nombres exactos que espera tu MAUI
            var progreso = ultimaEvaluacion.Detalles.Select(d => new
            {
                CriterioId = d.CriterioId,
                NombreCriterio = d.Criterio != null ? d.Criterio.Nombre : "Sin Criterio",

                // Extraemos el código corto (Ej: "1", "EP", "L") para tu convertidor de colores
                CodigoLogro = d.Nivel != null ? d.Nivel.Codigo : "N/A",

                Observacion = d.ObservacionEspecifica
            }).ToList();

            return Ok(progreso);
        }
    }
}