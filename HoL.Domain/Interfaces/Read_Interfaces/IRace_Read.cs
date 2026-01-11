using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Interfaces.Read_Interfaces
{
    public interface IRace_Read
    {
        int RaceId { get; }
        string RaceName { get; }
        string RaceDescription { get; }
        string RaceHistory { get; }
        RaceCategory RaceCategory { get; }
        List<string>? RaceHierarchySystem { get; }
        ConvictionType Conviction { get; }
        Treasure? Treasure { get; }
        int BaseXP { get; }
        BodyDimension BodyDimensins { get; }
        Dictionary<MobilityType, int> Mobility { get; }
        int FightingSpiritNumber { get; }
        int ZSM { get; }
        int DomesticationValue { get; }
        int BaseInitiative { get; }
        List<SpecialAbilities>? SpecialAbilities { get; }
        Dictionary<BodyStat, ValueRange> StatsPrimar { get; }
        Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; }
    }
}

