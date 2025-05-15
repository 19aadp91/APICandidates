namespace Domain.Services.Candidates.Dtos
{
    public class CreateCandidate
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public List<CreateCandidateExperienceCandidate>? Experiences { get; set; } = null!;
    }
}
