using HoL.Domain.Enums;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Aplication.DTOs.AnatomiDtos;
using HoL.Aplication.DTOs.StatDtos;
using System.Text;
using System.Diagnostics;

namespace HoL.Aplication.DTOs.EntitiDtos
{
    /// <summary>
    /// DTO pro entitu Race (rasa).
    /// </summary>
    [DebuggerDisplay("(ID: {Id} RaceDto: {RaceName} )")]
    public class RaceDto
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; } = string.Empty;
        public string RaceDescription { get; set; } = string.Empty;
        public string RaceHistory { get; set; } = string.Empty;
        public RaceCategory RaceCategory { get; set; }
        public List<string> RaceHierarchySystem { get; set; } = new List<string>();
        public Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; } = new() 
        {
            { VulnerabilityType.AcidOrPotion, 1 },
            { VulnerabilityType.BlessedArrow, 1 },
            { VulnerabilityType.BluntForce, 1 },
            { VulnerabilityType.PhysicalSpells, 1 },
            { VulnerabilityType.MentalSpells, 1 },
            { VulnerabilityType.ElementalSpells, 1 },
            { VulnerabilityType.MagicWeapon, 1 },
            { VulnerabilityType.SharpForce, 1 },
            { VulnerabilityType.Domination, 1 },
            { VulnerabilityType.HolyWater, 1 },
            { VulnerabilityType.PsychicAttack, 1 },
            { VulnerabilityType.RangerSpellsI, 1 },
            { VulnerabilityType.SharpForce, 1 },
            { VulnerabilityType.SpecificWeapon, 1 },
        };
        public ConvictionType Conviction { get; set; }
        public CurrencyDto Treasure { get; set; } = new();
        public int BaseXP { get; set; }
        public AnatomyProfileDto BodyDimensins { get; set; } = new();
        public Dictionary<MobilityType, int> Mobility { get; set; } = new();
        public FightingSpiritDto FightingSpirit { get; set; } = new();
        public WeaponDto RaceWeapon { get; set; } = new();
        public int ZSM { get; set; }
        public int DomesticationValue { get; set; }
        public int BaseInitiative { get; set; }
        public List<SpecialAbilitiesDto> SpecialAbilities { get; set; } = new();
        public Dictionary<BodyStat, ValueRangeDto> StatsPrimar { get; set; } = new();

        public override string? ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Id: {RaceId}");
            sb.AppendLine($"RaceName: {RaceName}");
            return sb.ToString() ;
        }
    }
}

