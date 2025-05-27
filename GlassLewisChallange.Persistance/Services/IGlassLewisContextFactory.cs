using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Persistance.Services
{
    public class IGlassLewisContextFactory : DesignTimeDbContextFactoryBase<GlassLewisContext>
    {
        public IGlassLewisContextFactory() { }
        protected override GlassLewisContext CreateNewInstance(DbContextOptions<GlassLewisContext> options)
        {
            return new GlassLewisContext(options);
        }
    }
}
