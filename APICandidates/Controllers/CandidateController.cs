using Domain.Services.Candidates.BusinessLogic;
using Domain.Services.Candidates.Dtos;
using Infrastructure.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICandidates.Controllers
{
    public class CandidateController(ICandidateServices candidateServices) : Controller
    {
        private readonly ICandidateServices _candidateServices = candidateServices;

        public async Task<IActionResult> Index()
        {
            var response = await _candidateServices.GetWhitFilter(new PageCommit<FilterCandidate> { PageIndex = 1, PageSize = 10 });

            return View(response);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCandidate model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Response = new Response<GetCandidate>() { Errors = [new() { Message = "El formulario no es valido" }] };
                return View(model);
            }

            var response = await _candidateServices.Create(model);

            if (response.Errors != null && response.Errors.Any())
            {
                ViewBag.Response = response;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string email)
        {
            var response = await _candidateServices.GetWhitFilter(new PageCommit<FilterCandidate> { PageIndex = 1, PageSize = 1, Filter = new() { Email = email} });

            var item = response.Body!.Items.FirstOrDefault();

            if (item == null) return NotFound();

            var updateModel = new UpdateCandidate
            {
                IdCandidate = item.IdCandidate,
                Name = item.Name,
                Surname = item.Surname,
                Email = item.Email,
                Birthdate = item.Birthdate,
                Experiences = item.Experiences.Select(exp => new UpdateCandidateExperienceCandidate
                {
                    IdCandidatesExperiences = exp.IdCandidatesExperiences,
                    IdCandidate = exp.IdCandidate,
                    Company = exp.Company,
                    Job = exp.Job,
                    Description = exp.Description,
                    Salary = exp.Salary,
                    BeginDate = exp.BeginDate,
                    EndDate = exp.EndDate
                }).ToList()
            };

            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCandidate model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Response = new Response<GetCandidate>() { Errors = [new() { Message = "El formulario no es valido" }] };
                return View(model);
            }

            var response = await _candidateServices.Update(model);

            if (response.Errors != null && response.Errors.Any())
            {
                ViewBag.Response = response;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _candidateServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
