using Domain.Services.Candidates.Dtos;
using Infrastructure.Utils.Models;

namespace Domain.Services.Candidates.BusinessLogic
{
    public interface ICandidateServices
    {
        Task<Response<GetCandidate>> Create(CreateCandidate create);
        Task<Response<GetCandidate>> Delete(int id);
        Task<Response<PagedResult<GetCandidate>>> GetWhitFilter(PageCommit<FilterCandidate> filter);
        Task<Response<GetCandidate>> Update(UpdateCandidate update);
    }
}
