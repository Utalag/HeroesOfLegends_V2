using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace HoL.Infrastructure.Data.ModelConfigurations
{
    public class RaceConfiguration : IEntityTypeConfiguration<Race>
    {
        public void Configure(EntityTypeBuilder<Race> builder)
        {
            builder.ToTable("Races");
            builder.HasKey(r => r.RaceId);

            // Basic properties
            builder.Property(r => r.RaceId).ValueGeneratedOnAdd();
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

            // Configure RaceHierarchySystem as owned collection
            builder.Property(r => r.RaceHierarchySystem)
                .HasColumnType("nvarchar(100)");


            // Configure AnatomyProfile as owned entity
            builder.OwnsOne(r => r.BodyDimensins);

            // Configure BodyParts collection
            builder.OwnsMany(r => r.BodyParts, bodyPart =>
            {
                bodyPart.ToTable("RaceBodyParts");
                bodyPart.WithOwner().HasForeignKey("RaceId");

                bodyPart.Property(bp => bp.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                bodyPart.Property(bp => bp.Type)
                    .IsRequired()
                    .HasConversion<string>();

                bodyPart.Property(bp => bp.Count)
                    .HasDefaultValue(1);

                bodyPart.Property(bp => bp.Function)
                    .HasMaxLength(500);

                bodyPart.Property(bp => bp.Appearance)
                    .HasMaxLength(500);

                bodyPart.Property(bp => bp.IsMagical)
                    .HasDefaultValue(false);


                bodyPart.OwnsOne(at=>at.Attack, attack =>
                {
                    attack.OwnsOne(a => a.DamageDice);
                });
                bodyPart.OwnsOne(df => df.Defense);

              
            });


            // Configure SpecialAbilities collection
            builder.OwnsMany(sa=>sa.SpecialAbilities).ToJson();

            // Configure Treasure as owned entity
            builder.OwnsOne(cu => cu.Treasure, treasure=>
            {
                treasure.HasOne(t=>t.Currency)
                .WithMany()
                .HasForeignKey(t=>t.CurrencyId)
                .IsRequired(false);
            });

            // Configure StatsPrimar as a JSON column
            // Configure StatsPrimar as JSON
            builder.Property(r => r.StatsPrimar)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<StatType, ValueRange>>(v, (JsonSerializerOptions)null) ?? new Dictionary<StatType, ValueRange>()
                );

            // Configure Mobility as JSON
            builder.Property(r => r.Mobility)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<MobilityType, int>>(v, (JsonSerializerOptions)null) ?? new Dictionary<MobilityType, int>()
                );

            // Configure Vulnerabilities as JSON
            builder.Property(r => r.Vulnerabilities)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<VulnerabilityType, double>>(v, (JsonSerializerOptions)null) ?? new Dictionary<VulnerabilityType, double>()
                );

        }
    }
}