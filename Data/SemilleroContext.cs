using Microsoft.EntityFrameworkCore;
using SemilleroGR3.API.Models;

namespace SemilleroGR3.API.Data
{
    public class SemilleroContext : DbContext
    {
        // 👇 Este es el constructor vital que enlaza el contexto con la configuración de SQL Server
        public SemilleroContext(DbContextOptions<SemilleroContext> options) : base(options)
        {
        }

        // Mapeo de las tablas
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Familia_Alumno> Familia_Alumno { get; set; }
        public DbSet<Actividad> Actividad { get; set; }
        public DbSet<TareaCasa_Seguimiento> TareaCasa_Seguimiento { get; set; }
        public DbSet<Rubrica_Criterio> Rubrica_Criterio { get; set; }
        public DbSet<Rubrica_Nivel> Rubrica_Nivel { get; set; }
        public DbSet<EvaluacionRubrica> EvaluacionRubrica { get; set; }
        public DbSet<EvaluacionRubrica_Detalle> EvaluacionRubrica_Detalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Claves primarias compuestas
            modelBuilder.Entity<Familia_Alumno>()
                .HasKey(fa => new { fa.FamiliaId, fa.AlumnoId });

            // Configuraciones adicionales si es necesario (Entity Framework suele deducir las relaciones básicas)
        }
    }
}