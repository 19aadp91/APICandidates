using DataAccess;
using Domain.Services.CandidateExperience.BusinessLogic;
using Domain.Services.Candidates.BusinessLogic;
using Infrastructure.Utils.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace APICandidates.Extension
{
    public static class ExtensionProgram
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer("Server=LAPTOP-6PTE51VD\\SQLEXPRESS;Database=CandidatesDB;Trusted_Connection=True;TrustServerCertificate=True;", b => b.MigrationsAssembly("APICandidates")));
            services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICandidateServices, CandidateServices>();
            services.AddScoped<ICandidateExperienceServices, CandidateExperienceServices>();
            return services;
        }
    }
}
