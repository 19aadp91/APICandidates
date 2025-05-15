using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Candidates.Dtos
{
    public class UpdateCandidateExperienceCandidate
    {
        public int IdCandidatesExperiences { get; set; }
        public int IdCandidate { get; set; }
        public string Company { get; set; } = null!;
        public string Job { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
