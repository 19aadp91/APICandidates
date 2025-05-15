using AutoMapper;
using DataAccess.Models;
using Domain.Services.Candidates.Dtos;

namespace Domain.Services.Candidates.Mapping
{
    public class MappingCandidate : Profile
    {
        public MappingCandidate()
        {
            CreateMap<CreateCandidate, TblCandidate>();
            CreateMap<CreateCandidateExperienceCandidate, TblCandidateExperience>(); 

            CreateMap<TblCandidate, GetCandidate>();
            CreateMap<TblCandidateExperience, GetCandidateExperienceCandidate>();

            CreateMap<UpdateCandidate, TblCandidate>();
        }
    }
}
