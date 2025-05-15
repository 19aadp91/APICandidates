using DataAccess;
using Domain.Services.CandidateExperience.BusinessLogic;
using Domain.Services.Candidates.BusinessLogic;
using Infrastructure.Utils.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace APICandidates.Extension
{
    public static class ExtensionProgram
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("APICandidates")));
            services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICandidateServices, CandidateServices>();
            services.AddScoped<ICandidateExperienceServices, CandidateExperienceServices>();
            return services;
        }
    }
}
