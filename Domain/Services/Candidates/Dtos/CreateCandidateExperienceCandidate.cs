namespace Domain.Services.Candidates.Dtos
{
    public class CreateCandidateExperienceCandidate
    {
        public string Company { get; set; } = null!;
        public string Job { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
