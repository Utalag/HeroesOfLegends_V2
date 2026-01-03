using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Entities
{

    public class Race
    {
        // optional properties
        public int Id { get; set; }
        public string RaceDescription { get; set; } = string.Empty;
        public string RaceHistory { get; set; } = string.Empty;
        public ConvictionType Conviction { get; set; } = ConvictionType.Neutral;
        public int ZSM { get; set; } = 0;
        public int DomesticationValue { get; set; } = 0;
        public int BaseInitiative { get; set; } = 0;
        public int BaseXP { get; set; }
        public int FightingSpiritNumber { get; set; }
        public Treasure? Treasure { get; set; }
        public List<BodyPart>? BodyParts { get; set; }
        public List<string>? RaceHierarchySystem { get; set; }
        public List<SpecialAbilities>? SpecialAbilities { get; set; }

        // required properties
        public string RaceName { get; private set; } = string.Empty;
        public RaceCategory RaceCategory { get; private set; }
        public BodyDimension BodyDimensins { get; private set; } = new();

        public Dictionary<BodyStat, ValueRange> StatsPrimar { get; set; } = new();
        public Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; } = new();
        public Dictionary<MobilityType, int> Mobility { get; set; } = new();


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

