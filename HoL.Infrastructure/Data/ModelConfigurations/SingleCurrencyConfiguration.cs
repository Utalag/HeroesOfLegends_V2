using HoL.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HoL.Infrastructure.Data.ModelConfigurations
{
    public class SingleCurrencyConfiguration : IEntityTypeConfiguration<SingleCurrencyDbModel>
    {
        public void Configure(EntityTypeBuilder<SingleCurrencyDbModel> builder)
        {
            builder.ToTable("SingleCurrencies");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ShotName)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(s => s.HierarchyLevel)
                .IsRequired();

            builder.Property(s => s.ExchangeRate)
                .IsRequired();

            // Unique index pro název měny
            builder.HasIndex(s => s.Name).IsUnique();
            
            // Index pro hierarchickou úroveň pro rychlé řazení
            builder.HasIndex(s => s.HierarchyLevel);
        }
    }
}
