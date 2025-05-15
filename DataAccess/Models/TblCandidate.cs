namespace DataAccess.Models
{
    public class TblCandidate
    {
        public int IdCandidate { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public ICollection<TblCandidateExperience> Experiences { get; set; } = [];
    }
}
