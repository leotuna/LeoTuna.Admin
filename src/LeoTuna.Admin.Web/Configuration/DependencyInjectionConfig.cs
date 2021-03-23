using LeoTuna.Admin.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LeoTuna.Admin.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<EmailService>();
            return services;
        }
    }
}
