namespace Domain.Services.CandidateExperience.Dtos
{
    public class UpdateCandidateExperience
    {
        public int IdCandidatesExperiences { get; set; }
        public string Company { get; set; } = null!;
        public string Job { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
