using Domain.Services.CandidateExperience.BusinessLogic;
using Domain.Services.CandidateExperience.Dtos;
using Infrastructure.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICandidates.Controllers
{
    public class CandidateExperienceController(ICandidateExperienceServices candidateExperienceServices) : Controller
    {
        private readonly ICandidateExperienceServices _candidateExperienceServices = candidateExperienceServices;

        public async Task<IActionResult> Index()
        {
            var response = await _candidateExperienceServices.GetWhitFilter(new PageCommit<FilterCandidateExperience> { PageIndex = 1, PageSize = 10 });
            return View(response.Body.Items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCandidateExperience model)
        {
            if (!ModelState.IsValid) return View(model);
            await _candidateExperienceServices.Create(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _candidateExperienceServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
