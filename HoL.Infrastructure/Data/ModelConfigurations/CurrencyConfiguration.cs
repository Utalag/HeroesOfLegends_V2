using HoL.Domain.Entities.CurencyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HoL.Infrastructure.Data.ModelConfigurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyGroup>
    {

        public void Configure(EntityTypeBuilder<CurrencyGroup> builder)
        {
            //builder.ToTable("Currencies");
            //builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id).ValueGeneratedOnAdd();
            //builder.Property(c => c.Currency1Name).IsRequired().HasMaxLength(50);
            //builder.Property(c => c.Currency2Name).IsRequired().HasMaxLength(50);
            //builder.Property(c => c.Currency3Name).IsRequired().HasMaxLength(50);
            //builder.Property(c => c.Currency4Name).IsRequired().HasMaxLength(50);
            //builder.Property(c => c.Currency5Name).IsRequired().HasMaxLength(50);

        }
    }
}
