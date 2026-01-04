using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Entities
{

    public class Race
    {
        // optional properties
        public int Id { get; internal set; }
        public string RaceDescription { get; internal set; } = string.Empty;
        public string RaceHistory { get; internal set; } = string.Empty;
        public ConvictionType Conviction { get; internal set; } = ConvictionType.Neutral;
        public int ZSM { get; internal set; } = 0;
        public int DomesticationValue { get; internal set; } = 0;
        public int BaseInitiative { get; internal set; } = 0;
        public int BaseXP { get; internal set; }
        public int FightingSpiritNumber { get; internal set; }
        public Treasure? Treasure { get; internal set; }
        public List<BodyPart>? BodyParts { get; internal set; }
        public List<string>? RaceHierarchySystem { get; internal set; }
        public List<SpecialAbilities>? SpecialAbilities { get; internal set; }

        // required properties
        public string RaceName { get; internal set; } = string.Empty;
        public RaceCategory RaceCategory { get; internal set; }
        public BodyDimension BodyDimensins { get; internal set; } = new();

        public Dictionary<BodyStat, ValueRange> StatsPrimar { get; internal set; } = new();
        public Dictionary<VulnerabilityType, double> Vulnerabilities { get; internal set; } = new();
        public Dictionary<MobilityType, int> Mobility { get; internal set; } = new();


        public Race(string raceName, RaceCategory raceCategory, BodyDimension bodyDimensions)
        {
            RaceName = raceName;
            RaceCategory = raceCategory;
            BodyDimensins = bodyDimensions;
            foreach (var enumTyp in Enum.GetValues(typeof(VulnerabilityType)))
            {
                Vulnerabilities.Add((VulnerabilityType)enumTyp, 1.0);
            }
            foreach (var enumTyp in Enum.GetValues(typeof(MobilityType)))
            {
                Mobility.Add((MobilityType)enumTyp, 10);
            }

        }

    }

}

