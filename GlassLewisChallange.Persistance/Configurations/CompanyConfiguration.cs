using GlassLewisChallange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlassLewisChallange.Persistance.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("COMPANIES");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnName("NAME").HasMaxLength(300).IsRequired();
            builder.Property(x => x.Exchange).HasColumnName("EXCHANGE").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Ticker).HasColumnName("TICKER").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Isin).HasColumnName("ISIN").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Website).HasColumnName("WEBSITE").HasMaxLength(600);

            builder.HasIndex(builder => builder.Isin);
        }
    }
}
