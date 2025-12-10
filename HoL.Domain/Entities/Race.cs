using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HoL.Domain.Enums;
using HoL.Domain.Interfaces.Read_Interfaces;
using HoL.Domain.Interfaces.Write_Interaces;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Entities
{

    public class Race //: IRace_Read, IRace_Write
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceId { get; set; }
        public string RaceName { get; set; } = string.Empty;
        public string RaceDescription { get; set; } = string.Empty;
        public string RaceHistory { get; set; } = string.Empty;
        public RaceCategory RaceCategory { get; set; }
        public ConvictionType Conviction { get; set; }
        public int ZSM { get; set; }
        public int DomesticationValue { get; set; }
        public int BaseInitiative { get; set; }
        public int BaseXP { get; set; }
        public int FightingSpiritNumber { get; set; } = new();


        public Treasure? Treasure { get; set; }
        public AnatomyProfile BodyDimensins { get; set; } = new();
        public List<BodyPart>? BodyParts { get; set; }
        public List<string>? RaceHierarchySystem { get; set; }
        public List<SpecialAbilities>? SpecialAbilities { get; set; }
        
        // Dictionary
        public Dictionary<StatType, ValueRange> StatsPrimar { get; set; } = new();

        //public Weapon? RaceWeapon { get; set; }

        public Dictionary<VulnerabilityType, double> Vulnerabilities { get; set; } = new();

      
        public Dictionary<MobilityType, int> Mobility { get; set; } = new();




    }

}

