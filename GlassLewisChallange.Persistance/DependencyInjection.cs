using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GlassLewisChallange.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");

            services.AddDbContext<GlassLewisContext>(options =>

              options.UseSqlServer(connectionString,
                              x =>
                              {
                                  x.MigrationsHistoryTable("__MigrationsHistoryOfGlassLewis");
                                  x.EnableRetryOnFailure();
                              })
              );

            services.AddScoped<IGlassLewisContext, GlassLewisContext>();

            return services;
        }
    }
}
