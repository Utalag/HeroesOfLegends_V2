using System.Collections.Generic;
using HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace;
using HoL.Aplication.DTOs.AnatomiDtos;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.DTOs.StatDtos;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace AplicationTest.ArrangeData
{
    //internal static class RaceDtoArrange
    //{
    //    public static RaceDto DefaultElf()
    //    {
    //        return new RaceDto
    //        {
    //            Id = 1,
    //            RaceName = "Elf",
    //            RaceDescription = "Elves are a mystical",
    //            RaceHistory = "Elves have a long history...",
    //            RaceCategory = RaceCategory.Humanoid,
    //            RaceHierarchySystem = new List<string> { "Wise", "Elder", "Warrior", "Novic" },
    //            Conviction = ConvictionType.Good,
    //            ZSM = 0,
    //            DomesticationValue = 0,
    //            BaseInitiative = 0,
    //            BaseXP = 100,

    //            Treasure = new TreasureDto
    //            {
    //                CurrencySetId = 1,
    //                Amounts = new Dictionary<int, int>
    //                {
    //                    [1] = 50,
    //                    [2] = 20,
    //                    [3] = 10
    //                }
    //            },

    //            BodyDimensins = new AnatomyProfileDto
    //            {
    //                HeightMin = 160,
    //                HeightMax = 200,
    //                WeightMin = 50,
    //                WeightMax = 100,
    //                RaceSize = RaceSize.B,
    //                MaxAge = 900
    //            },


    //            SpecialAbilities = new List<SpecialAbilitiesDto>
    //            {
    //                new SpecialAbilitiesDto
    //                {
    //                    AbilityName = "Night Vision",
    //                    AbilityDescription = "Elves can see in the dark up to 60 feet."
    //                },
    //                new SpecialAbilitiesDto
    //                {
    //                    AbilityName = "Keen Senses",
    //                    AbilityDescription = "Elves have advantage on Perception checks."
    //                }
    //            },

    //            StatsPrimar = new Dictionary<BodyStat, ValueRangeDto>
    //            {
    //                [BodyStat.Strength]     = new ValueRangeDto { DiceCount = 1, DiceType = DiceType.D6, Min = 6 },
    //                [BodyStat.Agility]      = new ValueRangeDto { DiceCount = 2, DiceType = DiceType.D6, Min = 8 },
    //                [BodyStat.Constitution] = new ValueRangeDto { DiceCount = 1, DiceType = DiceType.D6, Min = 5 },
    //                [BodyStat.Intelligence] = new ValueRangeDto { DiceCount = 2, DiceType = DiceType.D6, Min = 9 },
    //                [BodyStat.Charisma]     = new ValueRangeDto { DiceCount = 1, DiceType = DiceType.D6, Min = 6 },
    //                [BodyStat.Visage]       = new ValueRangeDto { DiceCount = 2, DiceType = DiceType.D6, Min = 8 },
    //            },

    //            Vulnerabilities = new Dictionary<VulnerabilityType, double>
    //            {
    //                [VulnerabilityType.SharpForce] = 1.2,
    //                [VulnerabilityType.MagicWeapon] = 1.1
    //            },

    //            Mobility = new Dictionary<MobilityType, int>
    //            {
    //                { MobilityType.Running, 30 },
    //                { MobilityType.Fly, 0 },
    //                { MobilityType.Swim, 3 }
    //            },

    //            FightingSpiritNumber = 2,
            
    //        };
    //    }

    //    public static RaceDto DefaultDragon()
    //    {
    //        return new RaceDto
    //        {
    //            Id = 1001,
    //            RaceName = "Dragon",
    //            RaceDescription = "Ancient, powerful, winged reptilian creature capable of flight and breath attacks.",
    //            RaceHistory = "Dragons have ruled mountains and skies for millennia, guarding vast hoards.",
    //            RaceCategory = RaceCategory.Dragon,
    //            Conviction = ConvictionType.Neutral,
    //            ZSM = 5,
    //            DomesticationValue = 0,
    //            BaseInitiative = 3,
    //            BaseXP = 1000,

    //            Treasure = new TreasureDto
    //            {
    //                CurrencySetId = 1,
    //                Amounts = new Dictionary<int, int>
    //                {
    //                    [1] = 5000,
    //                    [2] = 1200,
    //                    [3] = 300
    //                }
    //            },

    //            BodyDimensins = new AnatomyProfileDto
    //            {
    //                HeightMin = 400,
    //                HeightMax = 900,
    //                WeightMin = 800,
    //                WeightMax = 3000,
    //                RaceSize = RaceSize.E,
    //                MaxAge = 2000
    //            },

    //            RaceHierarchySystem = new List<string>
    //            {
    //                "Wyrmling", "Young", "Adult", "Ancient"
    //            },

    //            SpecialAbilities = new List<SpecialAbilitiesDto>
    //            {
    //                new SpecialAbilitiesDto
    //                {
    //                    AbilityName = "Fire Breath",
    //                    AbilityDescription = "Exhales a cone of fire that scorches everything in its path."
    //                },
    //                new SpecialAbilitiesDto
    //                {
    //                    AbilityName = "Flight",
    //                    AbilityDescription = "Can fly swiftly with strong wings."
    //                },
    //                new SpecialAbilitiesDto
    //                {
    //                    AbilityName = "Frightful Presence",
    //                    AbilityDescription = "Creatures within range must succeed on a fear check or become frightened."
    //                }
    //            },

    //            StatsPrimar = new Dictionary<BodyStat, ValueRangeDto>
    //            {
    //                [BodyStat.Strength] = new ValueRangeDto { DiceCount = 4, DiceType = DiceType.D6, Min = 16 },
    //                [BodyStat.Agility] = new ValueRangeDto { DiceCount = 3, DiceType = DiceType.D6, Min = 12 },
    //                [BodyStat.Constitution] = new ValueRangeDto { DiceCount = 4, DiceType = DiceType.D6, Min = 16 },
    //                [BodyStat.Intelligence] = new ValueRangeDto { DiceCount = 3, DiceType = DiceType.D6, Min = 12 },
    //                [BodyStat.Charisma] = new ValueRangeDto { DiceCount = 2, DiceType = DiceType.D6, Min = 10 },
    //                [BodyStat.Visage] = new ValueRangeDto { DiceCount = 3, DiceType = DiceType.D6, Min = 14 },
    //                [BodyStat.Luck] = new ValueRangeDto { DiceCount = 2, DiceType = DiceType.D6, Min = 10 },
    //            },

    //            Vulnerabilities = new Dictionary<VulnerabilityType, double>
    //            {
    //                [VulnerabilityType.SharpForce] = 0.8,
    //                [VulnerabilityType.BluntForce] = 1.1,
    //                [VulnerabilityType.MagicWeapon] = 1.3,
    //                [VulnerabilityType.ElementalSpells] = 1.2,
    //                [VulnerabilityType.HolyWater] = 0.5
    //            },

    //            Mobility = new Dictionary<MobilityType, int>
    //            {
    //                { MobilityType.Running, 20 },
    //                { MobilityType.Fly, 60 },
    //                { MobilityType.Swim, 10 }
    //            },

    //            FightingSpiritNumber = 10,
    //        };
    //    }
    //}
}
