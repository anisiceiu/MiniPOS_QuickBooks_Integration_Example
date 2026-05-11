using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniPOS.Application.Interfaces;
using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Infrastructure.Persistence;
using MiniPOS.Infrastructure.QuickBooks;

namespace MiniPOS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(
                provider => provider.GetRequiredService<ApplicationDbContext>());


            services.Configure<QuickBooksOptions>(
            configuration.GetSection("QuickBooks"));

            services.AddHttpClient<QuickBooksAuthService>();
            services.AddHttpClient<QuickBooksHttpClient>();

            services.AddScoped<IQuickBooksAuthService, QuickBooksAuthService>();
            services.AddScoped<IQuickBooksService, QuickBooksService>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }
    }
}
