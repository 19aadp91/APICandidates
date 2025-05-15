using AutoMapper;
using DataAccess.Models;
using Domain.Services.CandidateExperience.Dtos;

namespace Domain.Services.CandidateExperience.Mapping
{
    public class MappingCandidateExperience : Profile
    {
        public MappingCandidateExperience()
        {
            CreateMap<CreateCandidateExperience, TblCandidateExperience>();

            CreateMap<TblCandidateExperience, GetCandidateExperience>();

            CreateMap<UpdateCandidateExperience, TblCandidateExperience>();
        }
    }
}
