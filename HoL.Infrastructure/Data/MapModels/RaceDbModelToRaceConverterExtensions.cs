using System.Text.Json;
using AutoMapper;
using HoL.Domain.Builders;
using HoL.Domain.Entities;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using HoL.Infrastructure.Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HoL.Infrastructure.Data.MapModels
{
    public class RaceDbModelToRaceConverter : ITypeConverter<RaceDbModel, Race>
    {

        public Race Convert(RaceDbModel source, Race destination, ResolutionContext context)
        {
            return source.MapToRace();
        }
    }

    /// <summary>
    /// Extension metody pro mapování RaceDbModel → Race
    /// </summary>
    public static class RaceDbModelToRaceConverterExtensions
    {
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public static Race MapToRace(this RaceDbModel source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "RaceDbModel nesmí být null.");

            // Mapování BodyDimension (owned entity)
            var bodyDimension = MapBodyDimension(source.BodyDimensins);

            // Použití RaceBuilder pro vytvoření Race
            var builder = new RaceBuilder()
                .WithId(source.Id)
                .WithName(source.RaceName)
                .WithCategory(source.RaceCategory)
                .WithBodyDimensions(bodyDimension)
                .WithDescription(source.RaceDescription, source.RaceHistory)
                .WithConviction(source.Conviction)
                .WithBaseInitiative(source.BaseInitiative)
                .WithBaseXP(source.BaseXP)
                .WithFightingSpirit(source.FightingSpiritNumber)
                .WithZSM(source.ZSM)
                .WithDomesticationValue(source.DomesticationValue);

            // BodyParts
            if (!string.IsNullOrEmpty(source.JsonBodyParts))
            {
                var bodyParts = DeserializeBodyParts(source.JsonBodyParts);
                if (bodyParts != null)
                {
                    foreach (var part in bodyParts)
                    {
                        builder.AddBodyPart(part);
                    }
                }
            }

            // Stats
            if (!string.IsNullOrEmpty(source.JsonBodyStats))
            {
                var stats = DeserializeBodyStats(source.JsonBodyStats);
                if (stats != null && stats.Count > 0)
                {
                    builder.AddStat(stats);
                }
            }

            // Vulnerabilities
            if (!string.IsNullOrEmpty(source.JsonVulnerabilities))
            {
                var vulnerabilities = DeserializeVulnerabilities(source.JsonVulnerabilities);
                if (vulnerabilities != null && vulnerabilities.Count > 0)
                {
                    builder.AddVulnerabilities(vulnerabilities);
                }
            }

            // Mobility
            if (!string.IsNullOrEmpty(source.JsonMobility))
            {
                var mobility = DeserializeMobility(source.JsonMobility);
                if (mobility != null && mobility.Count > 0)
                {
                    builder.AddMobility(mobility);
                }
            }

            // RaceHierarchy
            if (!string.IsNullOrEmpty(source.JsonHierarchySystem))
            {
                var hierarchy = DeserializeHierarchySystem(source.JsonHierarchySystem);
                if (hierarchy != null && hierarchy.Count > 0)
                {
                    builder.AddRaceHierarchy(hierarchy);
                }
            }

            // SpecialAbilities
            if (!string.IsNullOrEmpty(source.JsonSpeciualAbilities))
            {
                var abilities = DeserializeSpecialAbilities(source.JsonSpeciualAbilities);
                if (abilities != null)
                {
                    foreach (var ability in abilities)
                    {
                        builder.AddSpecialAbility(ability);
                    }
                }
            }

            // Treasure
            if (source.Treasure != null && source.Treasure.CurrencyGroup != null)
            {
                var treasure = TreasureConverterExtensions.MapToTreasure(source.Treasure);

                if (treasure != null)
                    builder.WithTreasure(treasure);
            }

            return builder.Build();
        }

        private static BodyDimension MapBodyDimension(BodyDimensionDbModel dbModel)
        {
            if (dbModel == null)
                throw new ArgumentNullException(nameof(dbModel));

            // Parse RaceSize enum z stringu
            if (!Enum.TryParse<RaceSize>(dbModel.RaceSize, out var raceSize))
            {
                raceSize = RaceSize.B; // Default fallback
            }

            var bodyDimension = new BodyDimension(raceSize);
            
            // Nastavení rozsahů
            if (dbModel.WeightMin > 0 && dbModel.WeightMax > 0)
            {
                bodyDimension.SetWeightRange(dbModel.WeightMin, dbModel.WeightMax);
            }

            if (dbModel.LengthMin > 0 && dbModel.LengthMax > 0)
            {
                bodyDimension.SetLengthRange(dbModel.LengthMin, dbModel.LengthMax);
            }

            if (dbModel.HeightMin > 0 && dbModel.HeightMax > 0)
            {
                bodyDimension.SetHeightRange(dbModel.HeightMin, dbModel.HeightMax);
            }

            return bodyDimension;
        }

        #region Deserializace JSON dat

        private static List<BodyPart>? DeserializeBodyParts(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<BodyPart>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static Dictionary<BodyStat, ValueRange>? DeserializeBodyStats(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<BodyStat, ValueRange>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static Dictionary<VulnerabilityType, double>? DeserializeVulnerabilities(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<VulnerabilityType, double>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static Dictionary<MobilityType, int>? DeserializeMobility(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<MobilityType, int>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static List<string>? DeserializeHierarchySystem(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<string>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static List<SpecialAbilities>? DeserializeSpecialAbilities(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<SpecialAbilities>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }


        #endregion
    }
}
