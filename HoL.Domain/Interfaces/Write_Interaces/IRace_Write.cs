using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Interfaces.Write_Interaces
{
    public interface IRace_Write
    {
        int RaceId { get; set; }
        string RaceName { get; set; }
        string RaceDescription { get; set; }
        string RaceHistory { get; set; }
        RaceCategory RaceCategory { get; set; }
        List<string>? RaceHierarchySystem { get; set; }
        ConvictionType Conviction { get; set; }
        Treasure Treasure { get; set; }
        int BaseXP { get; set; }
        BodyDimension BodyDimensins { get; set; }
        Dictionary<MobilityType, int> Mobility { get; }
        int FightingSpiritNumber { get; set; }
        Weapon RaceWeapon { get; set; }
        int ZSM { get; set; }
        int DomesticationValue { get; set; }
        int BaseInitiative { get; set; }
        List<SpecialAbilities>? SpecialAbilities { get; set; }
        Dictionary<BodyStat, ValueRange> StatsPrimar { get; set; }
        Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; }
    }

}

