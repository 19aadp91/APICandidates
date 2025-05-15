using Domain.Services.CandidateExperience.Dtos;
using Infrastructure.Utils.Models;

namespace Domain.Services.CandidateExperience.BusinessLogic
{
    public interface ICandidateExperienceServices
    {
        Task<Response<GetCandidateExperience>> Create(CreateCandidateExperience create);
        Task<Response<GetCandidateExperience>> Delete(int id);
        Task<Response<PagedResult<GetCandidateExperience>>> GetWhitFilter(PageCommit<FilterCandidateExperience> filter);
        Task<Response<GetCandidateExperience>> Update(UpdateCandidateExperience update);
    }
}
