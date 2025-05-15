using System.ComponentModel.DataAnnotations;

namespace Application.Common.DTOs
{
    /// <summary>
    /// Representa un candidato con sus datos personales y experiencias laborales.
    /// </summary>
    public class CandidateDto
    {
        /// <summary>
        /// Identificador único del candidato.
        /// </summary>
        public int IdCandidate { get; set; }

        /// <summary>
        /// Nombre del candidato.
        /// </summary>
        /// <remarks>Longitud máxima: 50 caracteres.</remarks>
        [StringLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Apellidos del candidato.
        /// </summary>
        /// <remarks>Longitud máxima: 150 caracteres.</remarks>
        [StringLength(150)]
        public required string Surname { get; set; }

        /// <summary>
        /// Fecha de nacimiento del candidato.
        /// </summary>
        public required DateTime Birthdate { get; set; }

        /// <summary>
        /// Correo electrónico del candidato.
        /// </summary>
        /// <remarks>Debe ser una dirección de correo válida. Longitud máxima: 250 caracteres.</remarks>
        [StringLength(250)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Fecha de creación del registro del candidato.
        /// </summary>
        public required DateTime InsertDate { get; set; }

        /// <summary>
        /// Fecha de última modificación del registro, opcional.
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Lista de experiencias laborales asociadas al candidato.
        /// </summary>
        public List<CandidateExperienceDto> Experiences { get; set; } = new List<CandidateExperienceDto>();
    }
}
