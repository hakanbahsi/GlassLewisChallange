using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Persistance.Services
{
    public class GlassLewisContext : DbContext, IGlassLewisContext
    {
        public GlassLewisContext(DbContextOptions<GlassLewisContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());

            CascadeForeignKeys(modelBuilder);
        }

        private void CascadeForeignKeys(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
