using GlassLewisChallange.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace GlassLewisChallange.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IIdProtector, IdProtector>();

            return services;
        }
    }
}
