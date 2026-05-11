using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using MiniPOS.Application.QuickBooks;

namespace MiniPOS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            //services.AddScoped<ICompanyService, CompanyService>();
            //services.AddScoped<ICandidateProfileService, CandidateProfileService>();
            //services.AddScoped<IJobService, JobService>();

            return services;
        }
    }
}
