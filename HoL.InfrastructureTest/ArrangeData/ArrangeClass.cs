using System.Text.Json;
using HoL.Domain.Builders;
using HoL.Domain.Entities;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;
using HoL.Infrastructure.Data.Models;

namespace HoL.InfrastructureTest.ArrangeData
{
    /// <summary>
    /// Třída pro přípravu testovacích dat (Arrange) pro jednotkové testy.
    /// Obsahuje statické metody pro vytváření vzorových datových sad.
    /// Slouží pro testování mapování a repository operací.
    /// </summary>
    internal static class ArrangeClass
    {
        #region Race Constants (pro synchronizaci mapovacích testů)

        // === ELF CONSTANTS ===
        private const int ELF_ID = 1;
        private const string ELF_NAME = "Elf";
        private const string ELF_DESCRIPTION = "Graceful and agile beings of nature";
        private const string ELF_HISTORY = "Elves are ancient beings with long history";
        private const int ELF_BASE_INITIATIVE = 15;
        private const int ELF_BASE_XP = 100;
        private const int ELF_FIGHTING_SPIRIT = 10;
        private const int ELF_ZSM = 12;
        private const int ELF_DOMESTICATION_VALUE = 5;
        private const string ELF_RACE_SIZE = "B";
        private const int ELF_WEIGHT_MIN = 50;
        private const int ELF_WEIGHT_MAX = 80;
        private const int ELF_HEIGHT_MIN = 160;
        private const int ELF_HEIGHT_MAX = 190;
        private const int ELF_LENGTH_MIN = 160;
        private const int ELF_LENGTH_MAX = 190;
        private const int ELF_MAX_AGE = 750;
        private const ConvictionType ELF_CONVICTION = ConvictionType.Good;
        private const RaceCategory ELF_CATEGORY = RaceCategory.Humanoid;

        private static readonly string ELF_STATS_JSON = @"{
  ""Strength"": {
    ""Min"": 5,
    ""DiceCount"": 3,
    ""DiceType"": 6
  },
  ""Agility"": {
    ""Min"": 10,
    ""DiceCount"": 1,
    ""DiceType"": 6
  }
}";

        private static readonly string ELF_VULNERABILITIES_JSON = @"{
  ""PsychicAttack"": 1.0,
  ""BluntForce"": 0.5
}";

        private static readonly string ELF_MOBILITY_JSON = @"{
  ""Running"": 10,
  ""Swim"": 2
}";

        private static readonly string ELF_HIERARCHY_JSON = @"[
  ""Mudrc"",
  ""Vladce"",
  ""Rytir""
]";

        private static readonly string ELF_ABILITIES_JSON = @"[
  {
    ""AbilityName"": ""Dlouhovekost"",
    ""AbilityDescription"": ""Ziji extremne dlouho""
  }
]";

        // === DRAGON CONSTANTS ===
        private const int DRAGON_ID = 2;
        private const string DRAGON_NAME = "Red Dragon";
        private const string DRAGON_DESCRIPTION = "Mighty and ancient dragons with scales of crimson fire. Known for their devastating fire breath and immense strength.";
        private const string DRAGON_HISTORY = "Dragons are primordial creatures born from elemental magic. Red Dragons are among the most powerful and feared.";
        private const int DRAGON_BASE_INITIATIVE = 20;
        private const int DRAGON_BASE_XP = 500;
        private const int DRAGON_FIGHTING_SPIRIT = 25;
        private const int DRAGON_ZSM = 18;
        private const int DRAGON_DOMESTICATION_VALUE = 0;
        private const string DRAGON_RACE_SIZE = "E";
        private const int DRAGON_WEIGHT_MIN = 5000;
        private const int DRAGON_WEIGHT_MAX = 10000;
        private const int DRAGON_HEIGHT_MIN = 500;
        private const int DRAGON_HEIGHT_MAX = 1000;
        private const int DRAGON_LENGTH_MIN = 600;
        private const int DRAGON_LENGTH_MAX = 1200;
        private const int DRAGON_MAX_AGE = 5000;
        private const ConvictionType DRAGON_CONVICTION = ConvictionType.Evil;
        private const RaceCategory Dragon_CATEGORY = RaceCategory.Dragon;

        private static readonly string DRAGON_STATS_JSON = @"{
  ""Strength"": {
    ""Min"": 18,
    ""DiceCount"": 4,
    ""DiceType"": 8
  },
  ""Constitution"": {
    ""Min"": 15,
    ""DiceCount"": 2,
    ""DiceType"": 8
  }
}";

        private static readonly string DRAGON_MOBILITY_JSON = @"{
  ""Fly"": 40,
  ""Running"": 15
}";

        private static readonly string DRAGON_ABILITIES_JSON = @"[
  {
    ""AbilityName"": ""Fire Breath"",
    ""AbilityDescription"": ""Vydechuje kouzlo ohne""
  },
  {
    ""AbilityName"": ""Dragon Scales"",
    ""AbilityDescription"": ""Prirodzene brneni""
  }
]";

        #endregion


        #region SingleCurrency

        /// <summary>
        /// Vytvoří seznam SingleCurrencyDbModel pro testy - databázové modely.
        /// Obsahuje 3 měny: Gold, Silver, Copper se vzestupným kurzem.
        /// </summary>
        public static List<SingleCurrencyDbModel> SingleCurrencyDbModel_Arrange()
        {
            var gold = new SingleCurrencyDbModel
            {
                Id = 1,
                Name = "Gold",
                ShotName = "gl",
                ExchangeRate = 1,
                HierarchyLevel = 1
            };

            var silver = new SingleCurrencyDbModel
            {
                Id = 2,
                Name = "Silver",
                ShotName = "sl",
                ExchangeRate = 10,
                HierarchyLevel = 2
            };

            var copper = new SingleCurrencyDbModel
            {
                Id = 3,
                Name = "Copper",
                ShotName = "cp",
                ExchangeRate = 100,
                HierarchyLevel = 3
            };

            return new List<SingleCurrencyDbModel> { gold, silver, copper };
        }

        /// <summary>
        /// Vytvoří seznam SingleCurrency pro testy - doménové modely.
        /// Obsahuje 3 měny: Gold, Silver, Copper se vzestupným kurzem.
        /// </summary>
        public static List<SingleCurrency> SingleCurrency_Arrange()
        {
            var gold = new SingleCurrency("Gold", "gl", 1, 1);
            gold.SetId(1);

            var silver = new SingleCurrency("Silver", "sl", 2, 10);
            silver.SetId(2);

            var copper = new SingleCurrency("Copper", "cp", 3, 100);
            copper.SetId(3);

            return new List<SingleCurrency> { gold, silver, copper };
        }

        #endregion

        #region CurrencyGroup

        /// <summary>
        /// Vytvoří CurrencyGroupDbModel pro testy - databázový model.
        /// Obsahuje skupinu měn: FantasyCoins (Gold, Silver, Copper).
        /// </summary>
        public static CurrencyGroupDbModel CurrencyGroupDbModel_Arrange()
        {
            var singleCurrencies = SingleCurrencyDbModel_Arrange();

            var fantasyGroup = new CurrencyGroupDbModel
            {
                Id = 1,
                GroupName = "FantasyCoins",
                Currencies = new List<SingleCurrencyDbModel>
                {
                    singleCurrencies[0], // Gold
                    singleCurrencies[1], // Silver
                    singleCurrencies[2]  // Copper
                }
            };

            return fantasyGroup;
        }

        /// <summary>
        /// Vytvoří CurrencyGroup pro testy - doménový model.
        /// Obsahuje skupinu měn: FantasyCoins (Gold, Silver, Copper).
        /// </summary>
        public static CurrencyGroup CurrencyGroup_Arrange()
        {
            var singleCurrencies = SingleCurrency_Arrange();

            var fantasyGroup = new CurrencyGroup("FantasyCoins", new List<SingleCurrency>
            {
                singleCurrencies[0], // Gold
                singleCurrencies[1], // Silver
                singleCurrencies[2]  // Copper
            });
            fantasyGroup.SetId(1);

            return fantasyGroup;
        }

        #endregion

        #region Treasure

        /// <summary>
        /// Vytvoří TreasureDbModel pro testy - databázový model.
        /// Obsahuje poklad s FantasyCoins a množstvím mincí.
        /// </summary>
        public static TreasureDbModel TreasureDbModel_Arrange()
        {
            var coinQuantities = new Dictionary<int, int>
            {
                { 1, 100 }, // 100 Gold
                { 2, 50 },  // 50 Silver
                { 3, 25 }   // 25 Copper
            };

            var treasure = new TreasureDbModel
            {
                CurrencyId = 1,
                CoinQuantitiesJson = JsonSerializer.Serialize(coinQuantities),
                CurrencyGroup = CurrencyGroupDbModel_Arrange()
            };

            return treasure;
        }

        /// <summary>
        /// Vytvoří Treasure pro testy - doménový model.
        /// Obsahuje poklad s FantasyCoins a množstvím mincí.
        /// </summary>
        public static Treasure Treasure_Arrange()
        {
            var currencyGroup = CurrencyGroup_Arrange();

            var treasure = new Treasure(currencyGroup);
            treasure.AddCoins(1, 100); // 100 Gold
            treasure.AddCoins(2, 50);  // 50 Silver
            treasure.AddCoins(3, 25);  // 25 Copper

            return treasure;
        }

        #endregion

        #region Race

        /// <summary>
        /// Vytvoří RaceDbModel seznam pro testy - databázové modely.
        /// Obsahuje 2 rasy: Elf (humanoid) a Red Dragon s kompletní JSON serializací.
        /// Zcela nezávislá metoda bez references na Race_Arrange().
        /// SYNCHRONIZOVÁNO: Používá stejné konstanty jako Race_Arrange() pro mapovací testy.
        /// </summary>
        public static List<RaceDbModel> RaceDbModel_Arrange()
        {
            // Rasa 1: Elf
            var elfTreasure = TreasureDbModel_Arrange();
            var elf = new RaceDbModel
            {
                Id = ELF_ID,
                RaceName = ELF_NAME,
                RaceCategory = ELF_CATEGORY,
                RaceDescription = ELF_DESCRIPTION,
                RaceHistory = ELF_HISTORY,
                Conviction = ELF_CONVICTION,
                BaseInitiative = ELF_BASE_INITIATIVE,
                BaseXP = ELF_BASE_XP,
                FightingSpiritNumber = ELF_FIGHTING_SPIRIT,
                ZSM = ELF_ZSM,
                DomesticationValue = ELF_DOMESTICATION_VALUE,
                Treasure = elfTreasure,
                BodyDimensins = new BodyDimensionDbModel
                {
                    RaceSize = ELF_RACE_SIZE,
                    WeightMin = ELF_WEIGHT_MIN,
                    WeightMax = ELF_WEIGHT_MAX,
                    HeightMin = ELF_HEIGHT_MIN,
                    HeightMax = ELF_HEIGHT_MAX,
                    LengthMin = ELF_LENGTH_MIN,
                    LengthMax = ELF_LENGTH_MAX,
                    MaxAge = ELF_MAX_AGE
                },
                JsonBodyParts = "[]",
                JsonBodyStats = ELF_STATS_JSON,
                JsonVulnerabilities = ELF_VULNERABILITIES_JSON,
                JsonMobility = ELF_MOBILITY_JSON,
                JsonHierarchySystem = ELF_HIERARCHY_JSON,
                JsonSpeciualAbilities = ELF_ABILITIES_JSON
            };

            // Rasa 2: Red Dragon - s vlastním Treasure objektem
            var dragonTreasure = TreasureDbModel_Arrange();
            var dragon = new RaceDbModel
            {
                Id = DRAGON_ID,
                RaceName = DRAGON_NAME,
                RaceCategory = Dragon_CATEGORY,
                RaceDescription = DRAGON_DESCRIPTION,
                RaceHistory = DRAGON_HISTORY,
                Conviction = DRAGON_CONVICTION,
                BaseInitiative = DRAGON_BASE_INITIATIVE,
                BaseXP = DRAGON_BASE_XP,
                FightingSpiritNumber = DRAGON_FIGHTING_SPIRIT,
                ZSM = DRAGON_ZSM,
                DomesticationValue = DRAGON_DOMESTICATION_VALUE,
                Treasure = dragonTreasure,
                BodyDimensins = new BodyDimensionDbModel
                {
                    RaceSize = DRAGON_RACE_SIZE,
                    WeightMin = DRAGON_WEIGHT_MIN,
                    WeightMax = DRAGON_WEIGHT_MAX,
                    HeightMin = DRAGON_HEIGHT_MIN,
                    HeightMax = DRAGON_HEIGHT_MAX,
                    LengthMin = DRAGON_LENGTH_MIN,
                    LengthMax = DRAGON_LENGTH_MAX,
                    MaxAge = DRAGON_MAX_AGE
                },
                JsonBodyParts = "[]",
                JsonBodyStats = DRAGON_STATS_JSON,
                JsonVulnerabilities = "{}",
                JsonMobility = DRAGON_MOBILITY_JSON,
                JsonHierarchySystem = "[]",
                JsonSpeciualAbilities = DRAGON_ABILITIES_JSON
            };

            return new List<RaceDbModel> { elf, dragon };
        }

        /// <summary>
        /// Vytvoří Race seznam pro testy pomocí RaceBuilder - doménové modely.
        /// Obsahuje 2 rasy: Elf (humanoid) a Red Dragon s kompletní konfigurací.
        /// SYNCHRONIZOVÁNO: Používá stejné konstanty jako RaceDbModel_Arrange() pro mapovací testy.
        /// </summary>
        public static List<Race> Race_Arrange()
        {
            // Rasa 1: Elf - vytvořena pomocí RaceBuilder
            var elfTreasure = Treasure_Arrange();
            var elfBodyDim = new BodyDimension(RaceSize.B);
            elfBodyDim.SetWeightRange(ELF_WEIGHT_MIN, ELF_WEIGHT_MAX);
            elfBodyDim.SetHeightRange(ELF_HEIGHT_MIN, ELF_HEIGHT_MAX);
            elfBodyDim.SetLengthRange(ELF_LENGTH_MIN, ELF_LENGTH_MAX);

            var elfStats = new Dictionary<BodyStat, ValueRange>();
            elfStats[BodyStat.Strength] = new ValueRange(5, 3, DiceType.D6);
            elfStats[BodyStat.Agility] = new ValueRange(10, 1, DiceType.D6);

            var elfMobility = new Dictionary<MobilityType, int>();
            elfMobility[MobilityType.Running] = 10;
            elfMobility[MobilityType.Swim] = 2;

            var elfVulnerabilities = new Dictionary<VulnerabilityType, double>();
            elfVulnerabilities[VulnerabilityType.PsychicAttack] = 1;
            elfVulnerabilities[VulnerabilityType.BluntForce] = 0.5;

            var elf = new RaceBuilder()
                .WithId(ELF_ID)
                .WithName(ELF_NAME)
                .WithCategory(ELF_CATEGORY)
                .WithBodyDimensions(elfBodyDim)
                .WithDescription(ELF_DESCRIPTION, ELF_HISTORY)
                .WithConviction(ELF_CONVICTION)
                .WithBaseInitiative(ELF_BASE_INITIATIVE)
                .WithBaseXP(ELF_BASE_XP)
                .WithFightingSpirit(ELF_FIGHTING_SPIRIT)
                .WithZSM(ELF_ZSM)
                .WithDomesticationValue(ELF_DOMESTICATION_VALUE)
                .WithTreasure(elfTreasure)
                .AddVulnerabilities(elfVulnerabilities)
                .AddSpecialAbility(new SpecialAbilities("Dlouhovekost", "Ziji extremne dlouho"))
                .AddRaceHierarchy(new List<string> { "Mudrc", "Vladce", "Rytir" })
                .AddMobility(elfMobility)
                .AddStat(elfStats)
                .Build();

            // Rasa 2: Red Dragon - vytvořena pomocí RaceBuilder s vlastním Treasure
            var dragonTreasure = Treasure_Arrange();
            var dragonBodyDim = new BodyDimension(RaceSize.E);
            dragonBodyDim.SetWeightRange(DRAGON_WEIGHT_MIN, DRAGON_WEIGHT_MAX);
            dragonBodyDim.SetHeightRange(DRAGON_HEIGHT_MIN, DRAGON_HEIGHT_MAX);
            dragonBodyDim.SetLengthRange(DRAGON_LENGTH_MIN, DRAGON_LENGTH_MAX);

            var dragonStats = new Dictionary<BodyStat, ValueRange>();
            dragonStats[BodyStat.Strength] = new ValueRange(18, 4, DiceType.D8);
            dragonStats[BodyStat.Constitution] = new ValueRange(15, 2, DiceType.D8);

            var dragonMobility = new Dictionary<MobilityType, int>();
            dragonMobility[MobilityType.Fly] = 40;
            dragonMobility[MobilityType.Running] = 15;

            var dragon = new RaceBuilder()
                .WithId(DRAGON_ID)
                .WithName(DRAGON_NAME)
                .WithCategory(Dragon_CATEGORY)
                .WithBodyDimensions(dragonBodyDim)
                .WithDescription(DRAGON_DESCRIPTION, DRAGON_HISTORY)
                .WithConviction(DRAGON_CONVICTION)
                .WithBaseInitiative(DRAGON_BASE_INITIATIVE)
                .WithBaseXP(DRAGON_BASE_XP)
                .WithFightingSpirit(DRAGON_FIGHTING_SPIRIT)
                .WithZSM(DRAGON_ZSM)
                .WithDomesticationValue(DRAGON_DOMESTICATION_VALUE)
                .WithTreasure(dragonTreasure)
                .AddStat(dragonStats)
                .AddMobility(dragonMobility)
                .AddSpecialAbility(new SpecialAbilities("Fire Breath", "Vydechuje kouzlo ohne"))
                .AddSpecialAbility(new SpecialAbilities("Dragon Scales", "Prirodzene brneni"))
                .Build();

            return new List<Race> { elf, dragon };
        }

        #endregion
    }
}
