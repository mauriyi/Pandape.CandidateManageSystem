using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DbContexts
{
    /// <summary>
    /// Contexto principal de EF Core para la aplicación.
    /// Configura el acceso a las entidades y relaciones del modelo de dominio.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe opciones para la configuración del contexto.
        /// Permite la inyección de dependencias y configuración externa.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet para acceder y manipular los candidatos en la base de datos.
        /// </summary>
        public DbSet<Candidate> Candidates { get; set; }

        /// <summary>
        /// DbSet para acceder y manipular las experiencias laborales vinculadas a candidatos.
        /// </summary>
        public DbSet<CandidateExperience> CandidateExperiences { get; set; }

        /// <summary>
        /// Configuración fluida de las relaciones y comportamientos del modelo.
        /// En este caso, configuración de la relación uno a muchos entre Candidate y CandidateExperience,
        /// con eliminación en cascada para mantener integridad referencial.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Experiences)
                .WithOne(e => e.Candidate)
                .HasForeignKey(e => e.IdCandidate)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
