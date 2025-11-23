using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Stat;

namespace HoL.Domain.Interfaces.Read_Interfaces
{
    public interface IRace_Read
{
    int RaceId { get; }
    string RaceName { get; }
    string RaceDescription { get; }
    string RaceHistory { get; }
    RaceCategory RaceCategory { get; }
    List<string> RaceHierarchySystem { get; }
    ConvictionType Conviction { get; }
    Currency Treasure { get; }
    int BaseXP { get; }
    AnatomyProfile BodyDimensins { get; }
    Dictionary<MobilityType, int> Mobility { get; }
    FightingSpirit FightingSpirit { get; }
    Weapon RaceWeapon { get; }
    int ZSM { get; }
    int DomesticationValue { get; }
    int BaseInitiative { get; }
    List<SpecialAbilities> SpecialAbilities { get; }
    Dictionary<StatType, ValueRange> StatsPrimar { get; }
    Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; }
    }

}

