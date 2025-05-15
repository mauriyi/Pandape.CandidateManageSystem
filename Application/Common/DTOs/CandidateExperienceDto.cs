using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Common.DTOs
{
    /// <summary>
    /// Representa una experiencia laboral de un candidato.
    /// </summary>
    public class CandidateExperienceDto
    {
        /// <summary>
        /// Nombre de la empresa donde trabajó el candidato.
        /// </summary>
        /// <remarks>Longitud máxima: 100 caracteres.</remarks>
        [StringLength(100)]
        public required string Company { get; set; }

        /// <summary>
        /// Puesto desempeñado en la empresa.
        /// </summary>
        /// <remarks>Longitud máxima: 100 caracteres.</remarks>
        [StringLength(100)]
        public required string Job { get; set; }

        /// <summary>
        /// Descripción detallada de las funciones y responsabilidades.
        /// </summary>
        /// <remarks>Longitud máxima: 4000 caracteres.</remarks>
        [StringLength(4000)]
        public required string Description { get; set; }

        /// <summary>
        /// Salario recibido en la experiencia laboral.
        /// </summary>
        /// <remarks>Decimal con precisión 8,2 para almacenar valores monetarios.</remarks>
        [Column(TypeName = "decimal(8, 2)")]
        public required decimal Salary { get; set; }

        /// <summary>
        /// Fecha de inicio de la experiencia laboral.
        /// </summary>
        public required DateTime BeginDate { get; set; }

        /// <summary>
        /// Fecha de fin de la experiencia laboral, opcional.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Fecha de creación del registro de la experiencia.
        /// </summary>
        public required DateTime InsertDate { get; set; }

        /// <summary>
        /// Fecha de última modificación del registro de la experiencia, opcional.
        /// </summary>
        public DateTime? ModifyDate { get; set; }
    }
}