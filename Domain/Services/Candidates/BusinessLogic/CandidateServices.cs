using AutoMapper;
using DataAccess;
using DataAccess.Models;
using Domain.Services.Candidates.Dtos;
using Infrastructure.Utils.Models;
using Infrastructure.Utils.UnitOfWork;

namespace Domain.Services.Candidates.BusinessLogic
{
    public class CandidateServices(IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper) : ICandidateServices
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<GetCandidate>> Create(CreateCandidate create)
        {
            try
            {
                var existing = (await _unitOfWork.Repository<TblCandidate>().GetAsync(x => x.Email == create.Email)).FirstOrDefault();

                if (existing != null) return new() { Errors = [new() { Message = "Ya existe un candidato con este correo electrónico" }] };

                if (create.Experiences != null && create.Experiences.Any())
                {
                    var error = ValidarExperienciasFechas(create.Experiences);
                    if (error != null)
                        return new() { Errors = [new() { Message = error }] };
                }

                var candidate = _mapper.Map<TblCandidate>(create);
                candidate.InsertDate = DateTime.Now;

                candidate.Experiences?.ToList().ForEach(x => x.InsertDate = DateTime.Now);

                await _unitOfWork.Repository<TblCandidate>().InsertAsync(candidate);
                await _unitOfWork.SaveAsync();

                return new() { Body = _mapper.Map<GetCandidate>(candidate) };
            }
            catch
            {
                return new() { Errors = [new() { Message = "Ha ocurrido un error procesando la solicitud" }] };
            }
        }

        public async Task<Response<GetCandidate>> Delete(int id)
        {
            try
            {
                var item = (await _unitOfWork.Repository<TblCandidate>().GetAsync(x => x.IdCandidate == id, includeProperties: "Experiences")).FirstOrDefault();

                if (item == null) return new() { Errors = [new() { Message = "No se ecncontro el candidato" }] };

                _unitOfWork.Repository<TblCandidateExperience>().DeleteRange(item.Experiences);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.Repository<TblCandidate>().DeleteAsync(item.IdCandidate);

                await _unitOfWork.SaveAsync();

                return new()
                {
                    Body = _mapper.Map<GetCandidate>(item)
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
            }
        }

        public async Task<Response<PagedResult<GetCandidate>>> GetWhitFilter(PageCommit<FilterCandidate> filter)
        {
            try
            {
                if (filter.PageIndex <= 0) return new() { Errors = [new() { Message = "El index tiene que ser mayor que 0" }] };

                if (filter.PageSize <= 0) return new() { Errors = [new() { Message = "El size tiene que ser mayor que 0" }] };

                var item = await _unitOfWork.Repository<TblCandidate>().GetPagedAsync(
                    x =>
                    (filter.Filter == null || filter.Filter.Name == null || x.Name.ToLower().Contains(filter.Filter.Name.ToLower())) &&
                    (filter.Filter == null || filter.Filter.Surname == null || x.Surname.ToLower().Contains(filter.Filter.Surname.ToLower())) &&
                    (filter.Filter == null || filter.Filter.Email == null || x.Email.ToLower().Contains(filter.Filter.Email.ToLower())),
                    orderBy: null,
                    includeProperties: "Experiences",
                    pageIndex: filter.PageIndex,
                    pageSize: filter.PageSize
                );

                return new()
                {
                    Body = new()
                    {
                        Items = _mapper.Map<List<GetCandidate>>(item.Items) ?? [],
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

        public async Task<Response<GetCandidate>> Update(UpdateCandidate update)
        {
            try
            {
                var candidate = (await _unitOfWork.Repository<TblCandidate>().GetAsync(
                    x => x.IdCandidate == update.IdCandidate && x.Email == update.Email,
                    includeProperties: "Experiences")).FirstOrDefault();

                if (candidate == null)
                    return new() { Errors = [new() { Message = "No se encontró el candidato" }] };

                if (update.Experiences != null && update.Experiences.Any())
                {
                    var error = ValidarExperienciasFechas(update.Experiences);
                    if (error != null)
                        return new() { Errors = [new() { Message = error }] };
                }

                var updatedExpIds = update.Experiences!.Select(e => e.IdCandidatesExperiences).ToHashSet();

                var existingExperiences = candidate.Experiences
                    .Where(e => updatedExpIds.Contains(e.IdCandidatesExperiences))
                    .ToList();

                var toDelete = candidate.Experiences
                    .Where(e => !updatedExpIds.Contains(e.IdCandidatesExperiences))
                    .ToList();

                var newExperienceList = update.Experiences.Select(e =>
                {
                    var existing = existingExperiences
                        .FirstOrDefault(ex => ex.IdCandidatesExperiences == e.IdCandidatesExperiences);

                    return new TblCandidateExperience
                    {
                        IdCandidatesExperiences = e.IdCandidatesExperiences,
                        IdCandidate = candidate.IdCandidate,
                        Company = e.Company,
                        Job = e.Job,
                        Description = e.Description,
                        Salary = e.Salary,
                        BeginDate = e.BeginDate,
                        EndDate = e.EndDate,
                        InsertDate = existing?.InsertDate ?? DateTime.Now,
                        ModifyDate = existing != null ? DateTime.Now : null
                    };
                }).ToList();

                _unitOfWork.Repository<TblCandidateExperience>().DeleteRange(toDelete);
                await _unitOfWork.SaveAsync();

                foreach (var exp in newExperienceList.Where(e => e.ModifyDate != null))
                    _unitOfWork.Repository<TblCandidateExperience>().Update(exp);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.Repository<TblCandidateExperience>().AddRangeAsync(newExperienceList.Where(e => e.ModifyDate == null));
                await _unitOfWork.SaveAsync();

                update.Experiences = null!;
                _mapper.Map(update, candidate);
                candidate.Experiences = newExperienceList;
                candidate.ModifyDate = DateTime.Now;

                _unitOfWork.Repository<TblCandidate>().Update(candidate);
                await _unitOfWork.SaveAsync();

                return new() { Body = _mapper.Map<GetCandidate>(candidate) };
            }
            catch
            {
                return new() { Errors = [new() { Message = "Ha ocurrido un error procesando la solicitud" }] };
            }
        }

        private string? ValidarExperienciasFechas<T>(List<T> experiences) where T : class
        {
            var getBeginDate = typeof(T).GetProperty("BeginDate");
            var getEndDate = typeof(T).GetProperty("EndDate");
            var getCompany = typeof(T).GetProperty("Company");

            var ordered = experiences
                .OrderBy(e => (DateTime)getBeginDate!.GetValue(e)!)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                var begin = (DateTime)getBeginDate!.GetValue(ordered[i])!;
                var end = (DateTime?)getEndDate!.GetValue(ordered[i]);
                var company = (string)getCompany!.GetValue(ordered[i])!;

                if (end.HasValue && end.Value.Date <= begin.Date)
                    return $"La experiencia en {company} tiene EndDate menor o igual que BeginDate.";

                if (i > 0)
                {
                    var prevEnd = (DateTime?)getEndDate!.GetValue(ordered[i - 1]) ?? DateTime.MaxValue;
                    if (begin < prevEnd)
                    {
                        var prevCompany = (string)getCompany!.GetValue(ordered[i - 1])!;
                        return $"Las experiencias en {prevCompany} y {company} se solapan.";
                    }
                }
            }
            return null;
        }

    }
}
