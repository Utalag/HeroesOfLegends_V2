using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using HoL.Domain.Helpers.AnatomiHelpers;
using HoL.Domain.Helpers;

namespace AplicationTest.ArrangeData
{
    internal static class RaceArrange
    {
        //public static Race Arrange_Elf()
        //{
        //    var elf = new Race()
        //    {
        //        Id = 1,
        //        RaceName = "Elf",
        //        RaceDescription = "Elves are a mystical",
        //        RaceHistory = "Elves have a long history...",
        //        RaceCategory = RaceCategory.Humanoid,
        //        Conviction = ConvictionType.Good,
        //        ZSM = 0,
        //        DomesticationValue = 0,
        //        BaseInitiative = 0,
        //        BaseXP = 100,
        //        FightingSpiritNumber = 0,
        //        //Treasure = new()
        //        //{
        //        //    CurrencySetId = 1,
        //        //    Amounts = new()
        //        //    {
        //        //        [1] = 50,
        //        //        [2] = 20,
        //        //        [3] = 10,
        //        //    }
        //        //},
        //        BodyDimensins = new BodyDimension()
        //        {
        //            HeightMin = 160,
        //            HeightMax = 200,
        //            WeightMin = 50,
        //            WeightMax = 100,
        //            RaceSize = RaceSize.B,
        //            MaxAge = 900
        //        },
        //        BodyParts = null,
        //        RaceHierarchySystem = new List<string> { "Wise", "Elder", "Warrior", "Novic" },
        //        SpecialAbilities = new List<SpecialAbilities>()
        //        {
        //            new SpecialAbilities()
        //            {
        //                AbilityName = "Night Vision",
        //                AbilityDescription = "Elves can see in the dark up to 60 feet."
        //            },
        //            new SpecialAbilities()
        //            {
        //                AbilityName = "Keen Senses",
        //                AbilityDescription = "Elves have advantage on Perception checks."
        //            }
        //        },
        //        StatsPrimar = new Dictionary<BodyStat, ValueRange>
        //        {
        //            [BodyStat.Strength] = new ValueRange { DiceCount = 1, DiceType = DiceType.D6, Min = 6 },
        //            [BodyStat.Agility] = new ValueRange { DiceCount = 2, DiceType = DiceType.D6, Min = 8 },
        //            [BodyStat.Constitution] = new ValueRange { DiceCount = 1, DiceType = DiceType.D6, Min = 5 },
        //            [BodyStat.Intelligence] = new ValueRange { DiceCount = 2, DiceType = DiceType.D6, Min = 9 },
        //            [BodyStat.Charisma] = new ValueRange { DiceCount = 1, DiceType = DiceType.D6, Min = 6 },
        //            [BodyStat.Visage] = new ValueRange { DiceCount = 2, DiceType = DiceType.D6, Min = 8 },
        //        },
        //        Vulnerabilities = new Dictionary<VulnerabilityType, double>
        //        {
        //            [VulnerabilityType.SharpForce] = 1.2,
        //            [VulnerabilityType.MagicWeapon] = 1.1
        //        },
        //        Mobility = new Dictionary<MobilityType, int>
        //        {
        //            { MobilityType.Running, 30 },
        //            { MobilityType.Fly, 0 },
        //            { MobilityType.Swim, 3 }
        //        }
        //    };
        //    return elf;
        //}
    

        //public static Race Arrange_Dragon()
        //{
        //    var dragon = new Race()
        //    {
        //        Id = 1001,
        //        RaceName = "Dragon",
        //        RaceDescription = "Ancient, powerful, winged reptilian creature capable of flight and breath attacks.",
        //        RaceHistory = "Dragons have ruled mountains and skies for millennia, guarding vast hoards.",
        //        RaceCategory = RaceCategory.Dragon,
        //        Conviction = ConvictionType.Neutral,
        //        ZSM = 5,
        //        DomesticationValue = 0,
        //        BaseInitiative = 3,
        //        BaseXP = 1000,
        //        FightingSpiritNumber = 10,

        //        Treasure = new Treasure
        //        {
        //            CurrencySetId = 1,
        //            Amounts = new()
        //            {
        //                [1] = 5000,
        //                [2] = 1200,
        //                [3] = 300,
        //            }
        //        },

        //        BodyDimensins = new BodyDimension
        //        {
        //            HeightMin = 400,
        //            HeightMax = 900,
        //            WeightMin = 800,
        //            WeightMax = 3000,
        //            RaceSize = RaceSize.E, // velmi velký
        //            MaxAge = 2000
        //        },

        //        BodyParts = new List<BodyPart>
        //        {
        //            new BodyPart {
        //                Name = "head",
        //                BodyPartCategory = BodyPartType.Head,
        //                Quantity = 1,
        //                Function = "Houses sensory organs and brain",
        //                Appearance = "Large with sharp eyes and nostrils",
        //                IsMagical = false,
        //                Attack = new BodyPartAttack()
        //                {
        //                    Initiative = 3,
        //                    DamageType = DamageType.Piercing | DamageType.Slashing,
        //                    CanBeUsedWithOtherAttacks = false,
        //                    DamageDice = new Dice
        //                    {
        //                       Quantity = 3,
        //                       Sides = DiceType.D6,
        //                       Bonus = 5
        //                    }

        //                },
        //                Defense = new BodyPartDefense()
        //                {
        //                    ArmorValue = 15,
        //                    IsVital = true,
        //                    IsProtected = true
        //                }
        //            }

        //        },

        //        RaceHierarchySystem = new List<string>
        //        {
        //            "Wyrmling", "Young", "Adult", "Ancient"
        //        },

        //        SpecialAbilities = new List<SpecialAbilities>
        //        {
        //            new SpecialAbilities
        //            {
        //                AbilityName = "Fire Breath",
        //                AbilityDescription = "Exhales a cone of fire that scorches everything in its path."
        //            },
        //            new SpecialAbilities
        //            {
        //                AbilityName = "Flight",
        //                AbilityDescription = "Can fly swiftly with strong wings."
        //            },
        //            new SpecialAbilities
        //            {
        //                AbilityName = "Frightful Presence",
        //                AbilityDescription = "Creatures within range must succeed on a fear check or become frightened."
        //            }
        //        },

        //        StatsPrimar = new Dictionary<BodyStat, ValueRange>
        //        {
        //            [BodyStat.Strength] = new ValueRange { DiceCount = 4, DiceType = DiceType.D6, Min = 16 },
        //            [BodyStat.Agility] = new ValueRange { DiceCount = 3, DiceType = DiceType.D6, Min = 12 },
        //            [BodyStat.Constitution] = new ValueRange { DiceCount = 4, DiceType = DiceType.D6, Min = 16 },
        //            [BodyStat.Intelligence] = new ValueRange { DiceCount = 3, DiceType = DiceType.D6, Min = 12 },
        //            [BodyStat.Charisma] = new ValueRange { DiceCount = 2, DiceType = DiceType.D6, Min = 10 },
        //            [BodyStat.Visage] = new ValueRange { DiceCount = 3, DiceType = DiceType.D6, Min = 14 },
        //            [BodyStat.Luck] = new ValueRange { DiceCount = 2, DiceType = DiceType.D6, Min = 10 },
        //        },

        //        Vulnerabilities = new Dictionary<VulnerabilityType, double>
        //        {
        //            [VulnerabilityType.SharpForce] = 0.8,       // odolnější vůči sečným zbraním
        //            [VulnerabilityType.BluntForce] = 1.1,       // mírně zranitelný na drtivé útoky
        //            [VulnerabilityType.MagicWeapon] = 1.3,      // citlivost na magické zbraně
        //            [VulnerabilityType.ElementalSpells] = 1.2,  // citlivost na elementální kouzla (např. led)
        //            [VulnerabilityType.HolyWater] = 0.5         // méně účinné
        //        },

        //        Mobility = new Dictionary<MobilityType, int>
        //        {
        //            { MobilityType.Running, 20 },
        //            { MobilityType.Fly, 60 },
        //            { MobilityType.Swim, 10 }
        //        }
        //    };

        //    return dragon;
        //}
    }
}
