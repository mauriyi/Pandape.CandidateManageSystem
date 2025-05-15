using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Representa la experiencia laboral de un candidato dentro del dominio.
    /// </summary>
    public class CandidateExperience
    {
        /// <summary>
        /// Identificador único autogenerado para la experiencia.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCandidateExperience { get; set; }

        /// <summary>
        /// Llave foránea para asociar la experiencia con un candidato.
        /// </summary>
        [ForeignKey("IdCandidate")]
        public int IdCandidate { get; set; }

        /// <summary>
        /// Nombre de la empresa donde se desempeñó el candidato.
        /// Máximo 100 caracteres para evitar datos excesivamente largos.
        /// </summary>
        [StringLength(100)]
        public required string Company { get; set; }

        /// <summary>
        /// Nombre del puesto o cargo desempeñado.
        /// </summary>
        [StringLength(100)]
        public required string Job { get; set; }

        /// <summary>
        /// Descripción detallada del rol.
        /// Se permite hasta 4000 caracteres para cubrir la información relevante.
        /// </summary>
        [StringLength(4000)]
        public required string Description { get; set; }

        /// <summary>
        /// Salario reportado en el puesto, con precisión decimal para manejo correcto de valores monetarios.
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public required decimal Salary { get; set; }

        /// <summary>
        /// Fecha de inicio del empleo.
        /// </summary>
        public required DateTime BeginDate { get; set; }

        /// <summary>
        /// Fecha de fin del empleo, puede ser nula.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Fecha en que se registró esta experiencia en el sistema.
        /// </summary>
        public required DateTime InsertDate { get; set; }

        /// <summary>
        /// Fecha de última modificación de esta entrada, puede ser nula.
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Propiedad de navegación para acceso directo al candidato asociado.
        /// Se utiliza virtual para soportar EF.
        /// </summary>
        public virtual Candidate Candidate { get; set; } = null!;
    }
}
