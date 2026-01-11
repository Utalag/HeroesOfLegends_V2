using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using HoL.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace HoL.Infrastructure.Data.ModelConfigurations
{
    public class RaceConfiguration : IEntityTypeConfiguration<RaceDbModel>
    {
        public void Configure(EntityTypeBuilder<RaceDbModel> builder)
        {
            builder.ToTable("Races");
            builder.HasKey(r => r.Id);

            // Basic properties
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.RaceName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.RaceDescription).HasMaxLength(1000);
            builder.Property(r => r.RaceHistory).HasMaxLength(2000);
            builder.Property(r => r.RaceCategory).IsRequired().HasConversion<string>();
            builder.Property(r => r.Conviction).HasConversion<string>();
            builder.Property(r => r.ZSM).HasDefaultValue(0);
            builder.Property(r => r.DomesticationValue).HasDefaultValue(0);
            builder.Property(r => r.BaseInitiative).HasDefaultValue(0);
            builder.Property(r => r.BaseXP).IsRequired();
            builder.Property(r => r.FightingSpiritNumber).HasDefaultValue(0);

            // Owned Entity - BodyDimensions
            builder.OwnsOne(r => r.BodyDimensins, bd =>
            {
                bd.Property(b => b.RaceSize).IsRequired().HasMaxLength(10);
                bd.Property(b => b.WeightMin).IsRequired();
                bd.Property(b => b.WeightMax).IsRequired();
                bd.Property(b => b.LengthMin).IsRequired();
                bd.Property(b => b.LengthMax).IsRequired();
                bd.Property(b => b.HeightMin).IsRequired();
                bd.Property(b => b.HeightMax).IsRequired();
                bd.Property(b => b.MaxAge).IsRequired();
            });

            // Owned Entity - Treasure (optional)
            builder.OwnsOne(r => r.Treasure, t =>
            {
                t.Property(tr => tr.CoinQuantitiesJson)
                    .HasColumnName("treasure_CoinQuantities")
                    .HasColumnType("nvarchar(max)");
                
                t.Property(tr => tr.CurrencyId)
                    .HasColumnName("treasure_CurrencyId");

                // Navigační vlastnost na CurrencyGroup
                t.HasOne(tr => tr.CurrencyGroup)
                    .WithMany()
                    .HasForeignKey(tr => tr.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict); // Pokud se smaže CurrencyGroup, nemazat Race
            });

            // JSON Serialized Collections
            builder.Property(r => r.JsonBodyParts)
                .HasColumnName("bodyParts")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(r => r.JsonBodyStats)
                .HasColumnName("bodyStats")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(r => r.JsonVulnerabilities)
                .HasColumnName("vulnerabilities")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(r => r.JsonMobility)
                .HasColumnName("mobility")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(r => r.JsonHierarchySystem)
                .HasColumnName("hierarchySystem")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(r => r.JsonSpeciualAbilities)
                .HasColumnName("speciualAbilities")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            // Indexes pro výkon
            builder.HasIndex(r => r.RaceName).IsUnique();
            builder.HasIndex(r => r.RaceCategory);
        }
    }
}