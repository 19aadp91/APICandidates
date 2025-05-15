using AutoMapper;
using DataAccess;
using DataAccess.Models;
using Domain.Services.CandidateExperience.Dtos;
using Infrastructure.Utils.Models;
using Infrastructure.Utils.UnitOfWork;

namespace Domain.Services.CandidateExperience.BusinessLogic
{
    public class CandidateExperienceServices(IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper) : ICandidateExperienceServices
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<GetCandidateExperience>> Create(CreateCandidateExperience create)
        {
            try
            {
                var item = (await _unitOfWork.Repository<TblCandidateExperience>().GetAsync(x => 
                x.BeginDate < create.BeginDate && x.EndDate> create.BeginDate &&
                x.BeginDate < create.EndDate && x.EndDate > create.EndDate
                )).FirstOrDefault();

                if (item != null) return new() { Errors = [new() { Message = "Tienes otra experiencia en ese intervalo de tiempo" }] };

                var createService = _mapper.Map<TblCandidateExperience>(create);

                await _unitOfWork.Repository<TblCandidateExperience>().InsertAsync(createService!);

                await _unitOfWork.SaveAsync();

                return new()
                {
                    Body = _mapper.Map<GetCandidateExperience>(createService)
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
            }
        }

        public async Task<Response<GetCandidateExperience>> Delete(int id)
        {
            try
            {
                var item = (await _unitOfWork.Repository<TblCandidateExperience>().GetAsync(x => x.IdCandidatesExperiences == id)).FirstOrDefault();

                if (item == null) return new() { Errors = [new() { Message = "No se ecncontro experiencias relacionadas" }] };

                await _unitOfWork.Repository<TblCandidateExperience>().DeleteAsync(item.IdCandidatesExperiences);

                await _unitOfWork.SaveAsync();

                return new()
                {
                    Body = _mapper.Map<GetCandidateExperience>(item)
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
            }
        }

        public async Task<Response<PagedResult<GetCandidateExperience>>> GetWhitFilter(PageCommit<FilterCandidateExperience> filter)
        {
            try
            {
                if (filter.PageIndex <= 0) return new() { Errors = [new() { Message = "El index tiene que ser mayor que 0" }] };

                if (filter.PageSize <= 0) return new() { Errors = [new() { Message = "El size tiene que ser mayor que 0" }] };

                var item = await _unitOfWork.Repository<TblCandidateExperience>().GetPagedAsync(
                    x =>
                    (filter.Filter == null || x.Candidate.Email.ToLower().Contains(filter.Filter.Email.ToLower())),
                    orderBy: null,
                    includeProperties: "Experiences",
                    pageIndex: filter.PageIndex,
                    pageSize: filter.PageSize
                );

                return new()
                {
                    Body = new()
                    {
                        Items = _mapper.Map<List<GetCandidateExperience>>(item.Items) ?? [],
                        TotalItems = item.TotalItems,
                        PageIndex = item.PageIndex,
                        PageSize = item.PageSize
                    }
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
            }
        }

        public async Task<Response<GetCandidateExperience>> Update(UpdateCandidateExperience update)
        {
            try
            {
                var item = (await _unitOfWork.Repository<TblCandidateExperience>().GetAsync(x => x.IdCandidatesExperiences == update.IdCandidatesExperiences)).FirstOrDefault();

                if (item == null) return new() { Errors = [new() { Message = "No se ecncontro experiencias relacionadas" }] };

                _mapper.Map(update, item);

                _unitOfWork.Repository<TblCandidateExperience>().Update(item);

                await _unitOfWork.SaveAsync();

                return new()
                {
                    Body = _mapper.Map<GetCandidateExperience>(item)
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
            }
        }
    }
}
