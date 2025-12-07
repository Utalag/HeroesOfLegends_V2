using System.Text.Json;
using HoL.Domain;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Stat;
using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data
{
    public class SqlDbContext : DbContext
    {


        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        public DbSet<Race> Races { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplikovat configurations z assembly - pokud máte IEntityTypeConfiguration soubory
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);

            // NEBO použít inline konfiguraci (ne obojí)
            // ConfigureRaceEntity(modelBuilder);
        }

        private void ConfigureRaceEntity(ModelBuilder modelBuilder)
        {
            var raceEntity = modelBuilder.Entity<Race>();

            raceEntity.HasKey(r => r.RaceId);

            raceEntity.Property(r => r.RaceName)
                .IsRequired()
                .HasMaxLength(100);

            raceEntity.Property(r => r.RaceDescription)
                .HasMaxLength(3000);

            raceEntity.Property(r => r.RaceHistory)
                .HasMaxLength(3000);

            raceEntity.Property(r => r.RaceCategory)
                .HasConversion<string>();

            raceEntity.Property(r => r.Conviction)
                .HasConversion<string>();

            raceEntity.Property(r => r.RaceHierarchySystem)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnType("nvarchar(max)");

            raceEntity.Property(r => r.Vulnerabilities)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<VulnerabilityType, double>>(v, (JsonSerializerOptions)null) ?? new Dictionary<VulnerabilityType, double>())
                .HasColumnType("nvarchar(max)");

            raceEntity.Property(r => r.Mobility)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<MobilityType, int>>(v, (JsonSerializerOptions)null) ?? new Dictionary<MobilityType, int>())
                .HasColumnType("nvarchar(max)");

            raceEntity.Property(r => r.SpecialAbilities)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<SpecialAbilities>>(v, (JsonSerializerOptions)null) ?? new List<SpecialAbilities>())
                .HasColumnType("nvarchar(max)");

            raceEntity.Property(r => r.StatsPrimar)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<StatType, ValueRange>>(v, (JsonSerializerOptions)null) ?? new Dictionary<StatType, ValueRange>())
                .HasColumnType("nvarchar(max)");

            // Zkontrolujte, že value objekty mají správné vlastnosti
            raceEntity.ComplexProperty(r => r.Treasure, treasureBuilder =>
            {
                treasureBuilder.Property(c => c.Gold).HasColumnName("Treasure_Gold");
                treasureBuilder.Property(c => c.Silver).HasColumnName("Treasure_Silver");
                treasureBuilder.Property(c => c.Copper).HasColumnName("Treasure_Copper");
            });


            raceEntity.ComplexProperty(r => r.BodyDimensins, anatomyBuilder =>
            {
            anatomyBuilder.Property(a => a.HeihtMin).HasColumnName("Body_HeihtMin");
            anatomyBuilder.Property(a => a.HeihtMax).HasColumnName("Body_HeihtMax");
            anatomyBuilder.Property(a => a.WeightMin).HasColumnName("Body_WeightMin");
            anatomyBuilder.Property(a => a.WeightMax).HasColumnName("Body_WeightMax");
            anatomyBuilder.Property(a => a.LengthMin).HasColumnName("Body_LengthMin");
            anatomyBuilder.Property(a => a.LengthMax).HasColumnName("Body_LengthMax");
            anatomyBuilder.Property(a => a.RaceSize).HasColumnName("Body_Size");
            anatomyBuilder.Property(a => a.MaxAge).HasColumnName("MaxAge");

                // BodyParts jako JSON v rámci ComplexProperty
            anatomyBuilder.Property(b => b.BodyParts)
                    .HasColumnName("Body_BodyParts")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<BodyPart>>(v, (JsonSerializerOptions)null))
                    .HasColumnType("nvarchar(max)");
            });

            //raceEntity.ComplexProperty(r => r.FightingSpirit, spiritBuilder =>
            //{
            //    spiritBuilder.Property(f => f.Courage).HasColumnName("FightingSpirit_Courage");
            //    spiritBuilder.Property(f => f.Aggression).HasColumnName("FightingSpirit_Aggression");
            //});

            //raceEntity.ComplexProperty(r => r.RaceWeapon, weaponBuilder =>
            //{
            //    weaponBuilder.Property(w => w.Name).HasColumnName("Weapon_Name").HasMaxLength(100);
            //    weaponBuilder.Property(w => w.Damage).HasColumnName("Weapon_Damage");
            //    weaponBuilder.Property(w => w.Range).HasColumnName("Weapon_Range");
            //});
        }
    }
}
