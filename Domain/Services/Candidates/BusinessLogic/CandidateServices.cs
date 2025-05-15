using AutoMapper;
using DataAccess;
using DataAccess.Models;
using Domain.Services.Candidates.Dtos;
using Infrastructure.Utils.Models;
using Infrastructure.Utils.UnitOfWork;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                var item = (await _unitOfWork.Repository<TblCandidate>().GetAsync(x => x.Email == create.Email)).FirstOrDefault();

                if (item != null) return new() { Errors = [new() { Message = "Ya existe un candidato con este correo electronico" }] };

                var createService = _mapper.Map<TblCandidate>(create);

                createService.InsertDate = DateTime.Now;

                if (create.Experiences != null)
                {
                    createService.Experiences.ToList().ForEach(x=> x.InsertDate = DateTime.Now);
                }

                await _unitOfWork.Repository<TblCandidate>().InsertAsync(createService!);

                await _unitOfWork.SaveAsync();

                return new()
                {
                    Body = _mapper.Map<GetCandidate>(createService)
                };
            }
            catch
            {
                return new() { Errors = [new() { Message = "ha ocurrido un error procesando la solicitud" }] };
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
                var item = (await _unitOfWork.Repository<TblCandidate>().GetAsync(
                    x => x.IdCandidate == update.IdCandidate &&
                    x.Email == update.Email, includeProperties: "Experiences"
                    )).FirstOrDefault();

                if (item == null) return new() { Errors = [new() { Message = "No se ecncontro el candidato" }] };

                var actualexperiences = item.Experiences.ToList().Where(x=> update.Experiences!.Select(x=> x.IdCandidatesExperiences).Contains(x.IdCandidatesExperiences)).ToList();

                var deleteExperiences = item.Experiences.ToList().Where(x => !actualexperiences.Select(x => x.IdCandidatesExperiences).Contains(x.IdCandidatesExperiences));

                var experiences = update.Experiences!.Select(x=>
                {
                    if (actualexperiences.Select(x=> x.IdCandidatesExperiences).Contains(x.IdCandidatesExperiences))
                    {

                        return new TblCandidateExperience()
                        {
                            IdCandidatesExperiences = x.IdCandidatesExperiences,
                            IdCandidate = item.IdCandidate,
                            Company = x.Company,
                            Job = x.Job,
                            Description = x.Description,
                            Salary = x.Salary,
                            BeginDate = x.BeginDate,
                            EndDate = x.EndDate,
                            InsertDate = actualexperiences.Where(y=> y.IdCandidatesExperiences == x.IdCandidatesExperiences).First().InsertDate,
                            ModifyDate = DateTime.Now,
                        };
                    }
                    else
                    {
                        return new TblCandidateExperience()
                        {
                            IdCandidatesExperiences = x.IdCandidatesExperiences,
                            IdCandidate = item.IdCandidate,
                            Company = x.Company,
                            Job = x.Job,
                            Description = x.Description,
                            Salary = x.Salary,
                            BeginDate = x.BeginDate,
                            EndDate = x.EndDate,
                            InsertDate = DateTime.Now,
                            ModifyDate = null,
                        };
                    }
                }).ToList();

                _unitOfWork.Repository<TblCandidateExperience>().DeleteRange(deleteExperiences);

                await _unitOfWork.SaveAsync();

                foreach (var item1 in experiences.Where(x => x.ModifyDate != null))
                {
                    _unitOfWork.Repository<TblCandidateExperience>().Update(item1);
                };

                await _unitOfWork.SaveAsync();

                await _unitOfWork.Repository<TblCandidateExperience>().AddRangeAsync(experiences.Where(x => x.ModifyDate == null));

                await _unitOfWork.SaveAsync();

                update.Experiences = null!;

                _mapper.Map(update, item);

                item.Experiences = null!;

                item.ModifyDate = DateTime.Now;

                _unitOfWork.Repository<TblCandidate>().Update(item);

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
    }
}
