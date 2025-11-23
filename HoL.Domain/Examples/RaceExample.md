# Race – Dva příklady použití (aktualizováno)

Aktualizace: `StatsPrimar` nyní používá `Dictionary<StatType, ValueRange>` místo `Stat` a `Mobility` používá `Dictionary<MobilityType, int>` (číselná rychlost / hodnota pohybu).

---

## Příklad 1: Elf (Humanoid – obratný lesní obyvatel)

```csharp
using HeroesOfLegends.DataLayer;
using HeroesOfLegends.DataLayer.Helpers;
using HeroesOfLegends.DataLayer.Helpers.AnatomiHelpers;
using HeroesOfLegends.DataLayer.ValueObjects;

var elf = new Race
{
    RaceId = 1,
    RaceName = "Elf",
    RaceDescription = "Vznešený obratný lesní obyvatel s dlouhým životem.",
    RaceHistory = "Elfové žijí stovky let v harmonii s přírodou. Studují magii a udržují rovnováhu v lesích.",
    
    // Kategorizace
    RaceCategory = RaceCategory.Humanoid,
    
    // Hierarchie rasy (sociální struktura)
    RaceHierarchySystem = new List<string> 
    { 
        "Stařešina lesa", 
        "Hraničář", 
        "Mág", 
        "Válečník", 
        "Učedník" 
    },
    
    // Základní hodnoty
    BaseXP = 50,
    BaseInitiative = 3,
    ZSM = 1,  // Základní společenská míra
    DomesticationValue = 0,  // Nelze domestikovat
    
    // Primární statistiky – ValueRange s kostkami
    StatsPrimar = new Dictionary<StatType, ValueRange>
    {
        // Síla: 6-16 (2d6 s minimem 6)
        [StatType.Strength] = new ValueRange 
        { 
            Min = 6, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Obratnost: 8-18 (vyšší - elfové jsou obratní)
        [StatType.Agility] = new ValueRange 
        { 
            Min = 8, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Výdrž: 6-16
        [StatType.Constitution] = new ValueRange 
        { 
            Min = 6, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Inteligence: 7-17 (vyšší - elfové jsou moudří)
        [StatType.Intelligence] = new ValueRange 
        { 
            Min = 7, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Charisma: 7-17 (vyšší - elfové jsou elegantní)
        [StatType.Charisma] = new ValueRange 
        { 
            Min = 7, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        }
    },
    
    // Pohybové schopnosti (číselné hodnoty pohybu)
    Mobility = new Dictionary<MobilityType, int>
    {
        [MobilityType.Running] = 30,  // 30m za kolo
        [MobilityType.Crowl] = 15,    // plazení pomalejší
        [MobilityType.Swim] = 20      // plavání
    },
    
    // Anatomický profil
    BodyDimensins = new AnatomyProfile
    {
        RaceSize = RaceSize.B,  // 1,5-2m
        WeightMin = 55,
        WeightMax = 85,
        BodyHeightMin = 160,
        BodyHeightMax = 195,
        MaxAge = 750,  // Dlouhý život
        // BodyParts není nutné pro standardní humanoidy
    },
    
    // Zranitelnosti
    Vulnerability = new VulnerabilityProfil
    {
        // Elfové jsou odolnější vůči magii, citlivější na fyzické útoky
        VulnerabilityModifiers = new Dictionary<VulnerabilityType, int>
        {
            [VulnerabilityType.BluntForce] = -1,      // Slabší proti tupým útokům
            [VulnerabilityType.SharpForce] = -1,      // Slabší proti sečným
            [VulnerabilityType.MentalSpells] = 2,     // Odolný vůči mentálním
            [VulnerabilityType.ElementalSpells] = 1   // Částečná odolnost proti elementům
        }
    },
    
    // Přesvědčení
    Conviction = new ConvictionType { },
    
    // Typická zbraň rasy
    RaceWeapon = new Weapon
    {
        WeaponName = "Dlouhý luk",
        WeaponDescription = "Elegantní elfský luk z jílovce",
        Damage = new Dice { Count = 1, Sides = DiceType.D8, Bonus = 0 },
        DamageType = DamageType.Piercing
    },
    
    // Bojový duch (pravděpodobnost zapojení do boje)
    FightingSpirit = new FightingSpirit 
    { 
        DangerNumber = 5  // Rozumný - bojuje když je třeba
    },
    
    // Speciální schopnosti rasy
    SpecialAbilities = new List<SpecialAbilities>
    {
        new SpecialAbilities 
        { 
            AbilityName = "Noční vidění", 
            AbilityDescription = "Elf vidí ve tmě až na 30 metrů jako za šera." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Lesní cit", 
            AbilityDescription = "Bonus +2 k orientaci a tichému pohybu v lese." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Dlouhověkost", 
            AbilityDescription = "Elfové žijí až 750 let a pomalu stárnou." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Odolnost vůči kouzlům", 
            AbilityDescription = "Bonus +1 k záchranným hodům proti magii." 
        }
    },
    
    // Typický poklad/majetek
    Treasure = new Currency
    {
        Gold = 100,
        Silver = 50,
        Copper = 0
    }
};

// ===== UKÁZKA POUŽITÍ =====

// Výpis všech statistik s min/max hodnotami
Console.WriteLine("=== ELFÍ STATISTIKY ===");
foreach (var kvp in elf.StatsPrimar)
{
    var stat = kvp.Key;
    var range = kvp.Value;
    Console.WriteLine($"{stat}: Min={range.Min}, Max={range.Max} " +
                      $"({range.DiceCount}d{(int)range.DiceType}, bonus={range.Dice.Bonus})");
}

// Výpis pohybových schopností
Console.WriteLine("\n=== POHYBLIVOST ===");
foreach (var move in elf.Mobility)
{
    Console.WriteLine($"{move.Key}: {move.Value}m za kolo");
}

// Výpis speciálních schopností
Console.WriteLine("\n=== SPECIÁLNÍ SCHOPNOSTI ===");
foreach (var ability in elf.SpecialAbilities)
{
    Console.WriteLine($"• {ability.AbilityName}: {ability.AbilityDescription}");
}

// Test bojového ducha
bool willFight = elf.FightingSpirit.IsFighting();
Console.WriteLine($"\nElf se zapojí do boje? {(willFight ? "Ano" : "Ne (ustupuje)")}");

// Výpis anatomie
Console.WriteLine($"\n=== ANATOMIE ===");
Console.WriteLine($"Velikost: {elf.BodyDimensins.RaceSize} ({elf.BodyDimensins.BodyHeightMin}-{elf.BodyDimensins.BodyHeightMax}cm)");
Console.WriteLine($"Váha: {elf.BodyDimensins.WeightMin}-{elf.BodyDimensins.WeightMax}kg");
Console.WriteLine($"Max. věk: {elf.BodyDimensins.MaxAge} let");
```

**Výstup:**
```
=== ELFÍ STATISTIKY ===
Strength: Min=6, Max=16 (2d6, bonus=4)
Agility: Min=8, Max=18 (2d6, bonus=6)
Constitution: Min=6, Max=16 (2d6, bonus=4)
Intelligence: Min=7, Max=17 (2d6, bonus=5)
Charisma: Min=7, Max=17 (2d6, bonus=5)

=== POHYBLIVOST ===
Running: 30m za kolo
Crowl: 15m za kolo
Swim: 20m za kolo

=== SPECIÁLNÍ SCHOPNOSTI ===
• Noční vidění: Elf vidí ve tmě až na 30 metrů jako za šera.
• Lesní cit: Bonus +2 k orientaci a tichému pohybu v lese.
• Dlouhověkost: Elfové žijí až 750 let a pomalu stárnou.
• Odolnost vůči kouzlům: Bonus +1 k záchranným hodům proti magii.

Elf se zapojí do boje? Ano

=== ANATOMIE ===
Velikost: B (160-195cm)
Váha: 55-85kg
Max. věk: 750 let
```

---

## Příklad 2: Červený drak (Monster – létající ničitel)

```csharp
using HeroesOfLegends.DataLayer;
using HeroesOfLegends.DataLayer.Helpers;
using HeroesOfLegends.DataLayer.Helpers.AnatomiHelpers;
using HeroesOfLegends.DataLayer.ValueObjects;

var redDragon = new Race
{
    RaceId = 2,
    RaceName = "Červený drak",
    RaceDescription = "Mocné ohnivé monstrum ovládající plamen a hromadící poklady.",
    RaceHistory = "Starobylí tvorové z doby prvního ohně. Žijí v horských jeskyních a terorizují okolní země.",
    
    // Kategorizace
    RaceCategory = RaceCategory.Dragon,
    
    // Hierarchie (sociální postavení draků)
    RaceHierarchySystem = new List<string> 
    { 
        "Dračí pán", 
        "Starý drak", 
        "Dospělý drak", 
        "Mladý drak", 
        "Mládě" 
    },
    
    // Základní hodnoty
    BaseXP = 1500,  // Velmi nebezpečný protivník
    BaseInitiative = 5,
    ZSM = 0,  // Neúčastní se společnosti
    DomesticationValue = 0,  // Nelze domestikovat
    
    // Primární statistiky – mimořádně vysoké
    StatsPrimar = new Dictionary<StatType, ValueRange>
    {
        // Síla: 18-33 (3d6 s minimem 18)
        [StatType.Strength] = new ValueRange 
        { 
            Min = 18, 
            DiceCount = 3, 
            DiceType = DiceType.D6 
        },
        // Obratnost: 10-20
        [StatType.Agility] = new ValueRange 
        { 
            Min = 10, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Výdrž: 16-26 (vysoká)
        [StatType.Constitution] = new ValueRange 
        { 
            Min = 16, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Inteligence: 12-22 (chytrý)
        [StatType.Intelligence] = new ValueRange 
        { 
            Min = 12, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        },
        // Charisma: 14-24 (impozantní)
        [StatType.Charisma] = new ValueRange 
        { 
            Min = 14, 
            DiceCount = 2, 
            DiceType = DiceType.D6 
        }
    },
    
    // Pohybové schopnosti
    Mobility = new Dictionary<MobilityType, int>
    {
        [MobilityType.Running] = 40,   // Rychlý běh
        [MobilityType.Fly] = 80,       // Velmi rychlý let
        [MobilityType.Crowl] = 20,     // Plazení
        [MobilityType.Swim] = 30       // Umí plavat
    },
    
    // Anatomický profil s detailními částmi těla
    BodyDimensins = new AnatomyProfile
    {
        RaceSize = RaceSize.E,  // 5-10m
        WeightMin = 2000,
        WeightMax = 5000,
        BodyHeightMin = 500,
        BodyHeightMax = 1000,
        MaxAge = 1000,  // Velmi dlouhý život
        
        // Detailní popis částí těla (pro bojové účely)
        BodyParts = new List<BodyPart>
        {
            new BodyPart
            {
                Name = "Dračí hlava",
                Type = BodyPartType.Head,
                Count = 1,
                Function = "Ohnivý dech, kousnutí",
                Attack = new BodyPartAttack
                {
                    DamageDice = new Dice { Count = 6, Sides = DiceType.D10, Bonus = 5 },
                    DamageType = DamageType.Fire,
                    Initiative = 3
                },
                Defense = new BodyPartDefense
                {
                    IsVital = true,
                    ArmorValue = 6,
                    IsProtected = true
                },
                Appearance = "Masivní šupinatá hlava s ostrými rohy a rudýma očima"
            },
            new BodyPart
            {
                Name = "Drápy",
                Type = BodyPartType.Claw,
                Count = 4,
                Function = "Roztrhání protivníka",
                Attack = new BodyPartAttack
                {
                    DamageDice = new Dice { Count = 3, Sides = DiceType.D8, Bonus = 4 },
                    DamageType = DamageType.Slashing,
                    Initiative = 2,
                    CanBeUsedWithOtherAttacks = true
                },
                Appearance = "Ostré černé drápy dlouhé jako meče"
            },
            new BodyPart
            {
                Name = "Křídla",
                Type = BodyPartType.Wing,
                Count = 2,
                Function = "Let a vířivé údery",
                Attack = new BodyPartAttack
                {
                    DamageDice = new Dice { Count = 2, Sides = DiceType.D6, Bonus = 2 },
                    DamageType = DamageType.Bludgeoning,
                    Initiative = 2
                },
                Appearance = "Obrovská kožená křídla rozpětí 15 metrů",
                IsMagical = true
            },
            new BodyPart
            {
                Name = "Ocas",
                Type = BodyPartType.Tail,
                Count = 1,
                Function = "Silný smetající útok",
                Attack = new BodyPartAttack
                {
                    DamageDice = new Dice { Count = 3, Sides = DiceType.D8, Bonus = 3 },
                    DamageType = DamageType.Bludgeoning,
                    Initiative = 1
                },
                Appearance = "Dlouhý šupinatý ocas s kostěnými ostny"
            }
        }
    },
    
    // Zranitelnosti
    Vulnerability = new VulnerabilityProfil
    {
        VulnerabilityModifiers = new Dictionary<VulnerabilityType, int>
        {
            [VulnerabilityType.BluntForce] = 3,       // Odolný
            [VulnerabilityType.SharpForce] = 2,       // Tvrdé šupiny
            [VulnerabilityType.ElementalForce] = 5,   // Odolný vůči ohni
            [VulnerabilityType.MagicWeapon] = 0,      // Normální
            [VulnerabilityType.AcidOrPotion] = -2     // Citlivý na kyselinu
        }
    },
    
    // Přesvědčení
    Conviction = new ConvictionType { },
    
    // Přírodní zbraň
    RaceWeapon = new Weapon
    {
        WeaponName = "Ohnivý dech",
        WeaponDescription = "Kužel plamenů sahající až 20 metrů",
        Damage = new Dice { Count = 10, Sides = DiceType.D10, Bonus = 10 },
        DamageType = DamageType.Fire
    },
    
    // Bojový duch (vždy bojuje)
    FightingSpirit = new FightingSpirit 
    { 
        DangerNumber = 12  // Velmi agresivní
    },
    
    // Speciální schopnosti
    SpecialAbilities = new List<SpecialAbilities>
    {
        new SpecialAbilities 
        { 
            AbilityName = "Ohnivý dech", 
            AbilityDescription = "Kužel ohně 20m dosah, 10d10+10 poškození, záchrana proti DEX." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Strašlivá aura", 
            AbilityDescription = "Bytosti do 30m musí házet proti strachu nebo utečou." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Legendární odolnost", 
            AbilityDescription = "3x denně může automaticky uspět v záchranném hodu." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Detekce pokladů", 
            AbilityDescription = "Magicky cítí zlato a drahokamy na velkou vzdálenost." 
        },
        new SpecialAbilities 
        { 
            AbilityName = "Ohnivá imunita", 
            AbilityDescription = "Imunní vůči všem ohnivým útokům a kouzlům." 
        }
    },
    
    // Typický poklad (draci hromadí bohatství)
    Treasure = new Currency
    {
        Gold = 50000,
        Silver = 10000,
        Copper = 5000
    }
};

// ===== UKÁZKA POUŽITÍ =====

Console.WriteLine("=== DRAČÍ STATISTIKY ===");
Console.WriteLine($"Síla: Max = {redDragon.StatsPrimar[StatType.Strength].Max}");
Console.WriteLine($"Výdrž: Max = {redDragon.StatsPrimar[StatType.Constitution].Max}");
Console.WriteLine($"Charisma: Max = {redDragon.StatsPrimar[StatType.Charisma].Max}");

Console.WriteLine("\n=== POHYBLIVOST ===");
foreach (var move in redDragon.Mobility)
{
    Console.WriteLine($"{move.Key}: {move.Value}m za kolo");
}

Console.WriteLine("\n=== ÚTOKY TĚLESNÍMI ČÁSTMI ===");
if (redDragon.BodyDimensins.BodyParts != null)
{
    foreach (var part in redDragon.BodyDimensins.BodyParts.Where(p => p.Attack != null))
    {
        var atk = part.Attack;
        Console.WriteLine($"• {part.Name}: {atk.DamageDice.Count}d{(int)atk.DamageDice.Sides}+{atk.DamageDice.Bonus} " +
                          $"({atk.DamageType}, iniciativa {atk.Initiative})");
    }
}

Console.WriteLine("\n=== SPECIÁLNÍ SCHOPNOSTI ===");
foreach (var ability in redDragon.SpecialAbilities)
{
    Console.WriteLine($"• {ability.AbilityName}");
}

Console.WriteLine($"\n=== ANATOMIE ===");
Console.WriteLine($"Velikost: {redDragon.BodyDimensins.RaceSize} ({redDragon.BodyDimensins.BodyHeightMin}-{redDragon.BodyDimensins.BodyHeightMax}cm)");
Console.WriteLine($"Váha: {redDragon.BodyDimensins.WeightMin}-{redDragon.BodyDimensins.WeightMax}kg");
Console.WriteLine($"Max. věk: {redDragon.BodyDimensins.MaxAge} let");
Console.WriteLine($"Získané XP za zabití: {redDragon.BaseXP}");
Console.WriteLine($"Poklad: {redDragon.Treasure.Gold} zlatých");
```

**Výstup:**
```
=== DRAČÍ STATISTIKY ===
Síla: Max = 33
Výdrž: Max = 26
Charisma: Max = 24

=== POHYBLIVOST ===
Running: 40m za kolo
Fly: 80m za kolo
Crowl: 20m za kolo
Swim: 30m za kolo

=== ÚTOKY TĚLESNÍMI ČÁSTMI ===
• Dračí hlava: 6d10+5 (Fire, iniciativa 3)
• Drápy: 3d8+4 (Slashing, iniciativa 2)
• Křídla: 2d6+2 (Bludgeoning, iniciativa 2)
• Ocas: 3d8+3 (Bludgeoning, iniciativa 1)

=== SPECIÁLNÍ SCHOPNOSTI ===
• Ohnivý dech
• Strašlivá aura
• Legendární odolnost
• Detekce pokladů
• Ohnivá imunita

=== ANATOMIE ===
Velikost: E (500-1000cm)
Váha: 2000-5000kg
Max. věk: 1000 let
Získané XP za zabití: 1500
Poklad: 50000 zlatých
```

---

## Poznámky k implementaci

### ValueRange a statistiky
- `Min` = minimální možná hodnota
- `Max` = automaticky vypočítáno jako `Min + DiceCount * (Sides - 1)`
- `Dice.Bonus` = automaticky `Min - DiceCount`

### Mobility
- Slovník `Dictionary<MobilityType, int>` obsahuje rychlosti v metrech za kolo
- Různé rasy mají různé typy pohybu

### BodyParts
- **Volitelné** - pro jednoduché humanoidy není nutné
- **Důležité pro monstra** - popisuje útoky různými částmi těla
- Každá část může mít `Attack` a `Defense` vlastnosti

### SpecialAbilities
- Seznam speciálních schopností specifických pro rasu
- Měl by obsahovat herní mechaniky i narativní popis

### Vulnerability
- Slovník modifikátorů zranitelnosti
- Kladná čísla = odolnost, záporná = slabost

### FightingSpirit.DangerNumber
- Čím vyšší číslo, tím agresivnější bytost
- Metoda `IsFighting()` hodí kostkami a porovná s touto hodnotou
