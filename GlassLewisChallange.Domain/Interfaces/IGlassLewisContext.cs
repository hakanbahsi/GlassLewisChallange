using GlassLewisChallange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GlassLewisChallange.Domain.Interfaces
{
    public interface IGlassLewisContext
    {
        public DbSet<Company> Companies { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        DatabaseFacade Database { get; }
    }
}
