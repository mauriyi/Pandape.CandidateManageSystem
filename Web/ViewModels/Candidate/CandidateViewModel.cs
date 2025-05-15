using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Candidate
{
    public class CandidateViewModel
    {

        public int IdCandidate { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(150)]
        public required string Surname { get; set; }

        public required DateTime Birthdate { get; set; }

        [StringLength(250)]
        [EmailAddress]
        public required string Email { get; set; }

        public required DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public List<CandidateExperienceViewModel> Experiences { get; set; } = new List<CandidateExperienceViewModel>();
    }
}
