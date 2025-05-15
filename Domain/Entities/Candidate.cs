using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un candidato dentro del sistema.
    /// Contiene información básica y su historial de experiencias laborales.
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Identificador único autogenerado para el candidato.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCandidate { get; set; }

        /// <summary>
        /// Nombre del candidato, con límite de 50 caracteres para control de tamaño.
        /// </summary>
        [StringLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Apellido(s) del candidato, con límite de 150 caracteres.
        /// </summary>
        [StringLength(150)]
        public required string Surname { get; set; }

        /// <summary>
        /// Fecha de nacimiento del candidato.
        /// </summary>
        public required DateTime Birthdate { get; set; }

        /// <summary>
        /// Email válido del candidato, validado con atributo específico.
        /// </summary>
        [StringLength(250)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Fecha en que se creó el registro.
        /// </summary>
        public required DateTime InsertDate { get; set; }

        /// <summary>
        /// Fecha de última modificación del registro, si existe.
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Colección de experiencias laborales asociadas al candidato.
        /// Inicializada para evitar null reference exceptions.
        /// Virtual para soporte de EF.
        /// </summary>
        public virtual ICollection<CandidateExperience> Experiences { get; set; } = [];
    }
}
