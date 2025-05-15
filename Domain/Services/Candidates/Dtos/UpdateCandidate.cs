namespace Domain.Services.Candidates.Dtos
{
    public class UpdateCandidate
    {
        public int IdCandidate { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public List<UpdateCandidateExperienceCandidate>? Experiences { get; set; } = null!;
    }
}
