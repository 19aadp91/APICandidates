namespace Domain.Services.Candidates.Dtos
{
    public class GetCandidate
    {
        public int IdCandidate { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public List<GetCandidateExperienceCandidate> Experiences { get; set; } = [];
    }
}
