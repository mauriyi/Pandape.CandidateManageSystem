
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.ViewModels.Candidate
{
    public class CandidateExperienceViewModel
    {

        [StringLength(100)]
        public required string Company { get; set; }

        [StringLength(100)]
        public required string Job { get; set; }

        [StringLength(4000)]
        public required string Description { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public required decimal Salary { get; set; }

        public required DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public required DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
