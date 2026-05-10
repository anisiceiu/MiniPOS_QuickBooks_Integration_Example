using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MiniPOS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IJobCategoryService, JobCategoryService>();
            //services.AddScoped<ICompanyService, CompanyService>();
            //services.AddScoped<ICandidateProfileService, CandidateProfileService>();
            //services.AddScoped<IJobService, JobService>();

            return services;
        }
    }
}
