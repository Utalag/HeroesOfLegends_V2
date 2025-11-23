# AnatomyProfile - Příklady použití

## Základní přehled

`AnatomyProfile` reprezentuje anatomický profil rasy nebo postavy. Používá flexibilní systém `BodyPart` pro popis jakéhokoliv tvora - od lidí přes zvířata až po monstra.

### Struktura

- **AnatomyProfile** - hlavní profil obsahující:
  - Základní rozměry (velikost, váha, výška)
  - Seznam `BodyPart` (volitelný, nullable)

- **BodyPart** - jednotlivá část těla s vlastnostmi:
  - `Name` - název části
  - `Type` - typ z enum `BodyPartType`
  - `Count` - počet těchto částí
  - `Function` - popis funkce
  - `Attack` - útočné schopnosti (volitelné)
  - `Defense` - obranné vlastnosti (volitelné)
  - `Appearance` - vizuální popis
  - `IsMagical` - je magická?

---

## Příklad 1: Lidská anatomie (základní)

```csharp
using HeroesOfLegends.DataLayer;
using HeroesOfLegends.DataLayer.Helpers;
using HeroesOfLegends.DataLayer.Helpers.AnatomiHelpers;

var humanAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.B,          // 1,5m - 2m
    WeightMin = 50,                 // kg
    WeightMax = 100,
    BodyHeightMin = 150,            // cm
    BodyHeightMax = 200,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Hlava", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Řízení těla, smysly",
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 0 
            }
        },
        new BodyPart 
        { 
            Name = "Ruce", 
            Type = BodyPartType.Arm, 
            Count = 2,
            Function = "Uchopení, manipulace s předměty"
        },
        new BodyPart 
        { 
            Name = "Nohy", 
            Type = BodyPartType.Leg, 
            Count = 2,
            Function = "Chůze, běh"
        },
        new BodyPart 
        { 
            Name = "Oči", 
            Type = BodyPartType.Eye, 
            Count = 2,
            Function = "Vidění"
        }
    }
};

Console.WriteLine($"Rasa: {humanAnatomy.RaceSize}");
Console.WriteLine($"Počet částí těla: {humanAnatomy.BodyParts?.Count ?? 0}");
```

---

## Příklad 2: Drak (monstrum s útoky)

```csharp
var dragonAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.E,          // 5m - 10m
    WeightMin = 2000,               // kg
    WeightMax = 5000,
    BodyHeightMin = 500,            // cm
    BodyHeightMax = 1000,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Dračí hlava", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Dech ohně, smysly",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 6, Sides = DiceType.D10, Bonus = 5 },
                DamageType = DamageType.Fire,
                Initiative = 3
            },
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 5,
                IsProtected = true
            },
            Appearance = "Šupinatá, červená, s rohy"
        },
        new BodyPart 
        { 
            Name = "Drápy", 
            Type = BodyPartType.Claw, 
            Count = 4,
            Function = "Útok, uchopení",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 2, Sides = DiceType.D8, Bonus = 4 },
                DamageType = DamageType.Slashing,
                Initiative = 2,
                CanBeUsedWithOtherAttacks = true
            },
            Appearance = "Ostré, černé, zakřivené"
        },
        new BodyPart 
        { 
            Name = "Křídla", 
            Type = BodyPartType.Wing, 
            Count = 2,
            Function = "Let, útok",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 2, Sides = DiceType.D6, Bonus = 2 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 1
            },
            Appearance = "Obrovská kožená křídla",
            IsMagical = true
        },
        new BodyPart 
        { 
            Name = "Ocas", 
            Type = BodyPartType.Tail, 
            Count = 1,
            Function = "Rovnováha, útok",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 3, Sides = DiceType.D8, Bonus = 3 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 1
            },
            Appearance = "Dlouhý, šupinatý, s hřebeny"
        }
    }
};

// Výpis všech útoků draka
foreach (var part in dragonAnatomy.BodyParts.Where(p => p.Attack != null))
{
    Console.WriteLine($"{part.Name}: {part.Attack.DamageDice.Count}d{(int)part.Attack.DamageDice.Sides}" +
                      $"+{part.Attack.DamageDice.Bonus} ({part.Attack.DamageType})");
}
```

**Výstup:**
```
Dračí hlava: 6d10+5 (Fire)
Drápy: 2d8+4 (Slashing)
Křídla: 2d6+2 (Bludgeoning)
Ocas: 3d8+3 (Bludgeoning)
```

---

## Příklad 3: Kůň (zvíře)

```csharp
var horseAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.C,          // 2m - 3m
    WeightMin = 400,
    WeightMax = 600,
    BodyHeightMin = 140,            // cm (kohoutek)
    BodyHeightMax = 180,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Hlava", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Smysly, příjem potravy"
        },
        new BodyPart 
        { 
            Name = "Nohy", 
            Type = BodyPartType.Leg, 
            Count = 4,
            Function = "Rychlá lokomoce, kop",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 1, Sides = DiceType.D6, Bonus = 2 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 1
            },
            Appearance = "Dlouhé, s kopyty"
        },
        new BodyPart 
        { 
            Name = "Ocas", 
            Type = BodyPartType.Tail, 
            Count = 1,
            Function = "Odhánění hmyzu",
            Appearance = "Dlouhé žíně"
        },
        new BodyPart 
        { 
            Name = "Uši", 
            Type = BodyPartType.Ear, 
            Count = 2,
            Function = "Vynikající sluch"
        }
    }
};

// Zjistit, zda má kůň útočné schopnosti
var hasAttacks = horseAnatomy.BodyParts.Any(p => p.Attack != null);
Console.WriteLine($"Má útočné schopnosti: {hasAttacks}");  // True (kopnutí)
```

---

## Příklad 4: Chobotnice (mořské monstrum)

```csharp
var octopusAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.D,          // 3m - 5m
    WeightMin = 150,
    WeightMax = 300,
    BodyHeightMin = 200,
    BodyHeightMax = 400,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Hlava/Tělo", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Centrální nervová soustava",
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 2
            },
            Appearance = "Měkké, šedozelené"
        },
        new BodyPart 
        { 
            Name = "Chapadla", 
            Type = BodyPartType.Tentacle, 
            Count = 8,
            Function = "Uchopení, útok, lokomoce",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 1, Sides = DiceType.D4, Bonus = 1 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 2,
                CanBeUsedWithOtherAttacks = true
            },
            Appearance = "Dlouhá, s přísavkami"
        },
        new BodyPart 
        { 
            Name = "Oči", 
            Type = BodyPartType.Eye, 
            Count = 2,
            Function = "Vynikající vidění pod vodou",
            Appearance = "Velké, vypouklé"
        },
        new BodyPart 
        { 
            Name = "Zobák", 
            Type = BodyPartType.Mouth, 
            Count = 1,
            Function = "Trhání potravy",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 1, Sides = DiceType.D6, Bonus = 0 },
                DamageType = DamageType.Piercing,
                Initiative = 1
            }
        }
    }
};

// Spočítat celkový počet útočných možností
int totalAttackParts = octopusAnatomy.BodyParts
    .Where(p => p.Attack != null)
    .Sum(p => p.Count);
    
Console.WriteLine($"Celkem útočných částí: {totalAttackParts}");  // 9 (8 chapadel + 1 zobák)
```

---

## Příklad 5: Hydra (multi-hlava monstrum)

```csharp
var hydraAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.E,
    WeightMin = 3000,
    WeightMax = 5000,
    BodyHeightMin = 800,
    BodyHeightMax = 1200,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Hlavy", 
            Type = BodyPartType.Head, 
            Count = 7,                      // 7 hlav!
            Function = "Útok kyselinou, kousnutí",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 2, Sides = DiceType.D8, Bonus = 3 },
                DamageType = DamageType.Acid,
                Initiative = 3,
                CanBeUsedWithOtherAttacks = true    // Každá hlava útočí samostatně
            },
            Defense = new BodyPartDefense 
            { 
                IsVital = false,            // Pokud odstraníte jednu, vyrostou dvě nové!
                ArmorValue = 3
            },
            Appearance = "Hadí hlavy, zelené šupiny"
        },
        new BodyPart 
        { 
            Name = "Tělo", 
            Type = BodyPartType.Other, 
            Count = 1,
            Function = "Nosná struktura",
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 5,
                IsProtected = true
            },
            Appearance = "Masivní, šupinaté"
        },
        new BodyPart 
        { 
            Name = "Ocas", 
            Type = BodyPartType.Tail, 
            Count = 1,
            Function = "Útok, rovnováha",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 3, Sides = DiceType.D6, Bonus = 4 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 1
            }
        }
    }
};

// Hydra může útočit všemi hlavami najednou
var heads = hydraAnatomy.BodyParts.First(p => p.Type == BodyPartType.Head);
Console.WriteLine($"Hydra má {heads.Count} hlav, každá útočí " +
                  $"{heads.Attack.DamageDice.Count}d{(int)heads.Attack.DamageDice.Sides}+{heads.Attack.DamageDice.Bonus}");
```

---

## Příklad 6: Magický elemental (abstraktní entita)

```csharp
var fireElementalAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.C,
    WeightMin = 0,                  // Nemá hmotnost!
    WeightMax = 0,
    BodyHeightMin = 180,
    BodyHeightMax = 250,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Ohnivé jádro", 
            Type = BodyPartType.Other, 
            Count = 1,
            Function = "Zdroj síly, životní energie",
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 0,
                IsProtected = false
            },
            Appearance = "Zářivá koule čistého ohně",
            IsMagical = true
        },
        new BodyPart 
        { 
            Name = "Ohnivé ruce", 
            Type = BodyPartType.Arm, 
            Count = 2,
            Function = "Útok, manipulace",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 3, Sides = DiceType.D6, Bonus = 0 },
                DamageType = DamageType.Fire,
                Initiative = 2
            },
            Appearance = "Formované z plamenů",
            IsMagical = true
        },
        new BodyPart 
        { 
            Name = "Ohnivé oči", 
            Type = BodyPartType.Eye, 
            Count = 2,
            Function = "Vidění skrze plamen",
            Appearance = "Bílé žhavé body",
            IsMagical = true
        }
    }
};

// Všechny části jsou magické
bool allMagical = fireElementalAnatomy.BodyParts.All(p => p.IsMagical);
Console.WriteLine($"Všechny části magické: {allMagical}");  // True
```

---

## Příklad 7: Centaur (hybridní bytost)

```csharp
var centaurAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.C,
    WeightMin = 500,
    WeightMax = 700,
    BodyHeightMin = 200,
    BodyHeightMax = 240,
    
    BodyParts = new List<BodyPart>
    {
        // Lidská část
        new BodyPart 
        { 
            Name = "Lidská hlava", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Inteligence, komunikace",
            Defense = new BodyPartDefense { IsVital = true }
        },
        new BodyPart 
        { 
            Name = "Lidské ruce", 
            Type = BodyPartType.Arm, 
            Count = 2,
            Function = "Uchopení, boj zbraněmi"
        },
        // Koňská část
        new BodyPart 
        { 
            Name = "Koňské nohy", 
            Type = BodyPartType.Leg, 
            Count = 4,
            Function = "Rychlý běh, kopání",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 2, Sides = DiceType.D6, Bonus = 3 },
                DamageType = DamageType.Bludgeoning,
                Initiative = 1
            }
        },
        new BodyPart 
        { 
            Name = "Koňský ocas", 
            Type = BodyPartType.Tail, 
            Count = 1,
            Function = "Odhánění hmyzu"
        }
    }
};

Console.WriteLine($"Centaur - hybrid s {centaurAnatomy.BodyParts.Count} typy částí těla");
```

---

## Příklad 8: Obří pavouk (hmyz)

```csharp
var giantSpiderAnatomy = new AnatomyProfile
{
    RaceSize = RaceSize.C,
    WeightMin = 100,
    WeightMax = 200,
    BodyHeightMin = 150,
    BodyHeightMax = 200,
    
    BodyParts = new List<BodyPart>
    {
        new BodyPart 
        { 
            Name = "Hlavohruď", 
            Type = BodyPartType.Head, 
            Count = 1,
            Function = "Centrální nervová soustava",
            Defense = new BodyPartDefense 
            { 
                IsVital = true,
                ArmorValue = 4,
                IsProtected = true
            },
            Appearance = "Chitinový pancíř"
        },
        new BodyPart 
        { 
            Name = "Oči", 
            Type = BodyPartType.Eye, 
            Count = 8,
            Function = "Vidění na 360 stupňů"
        },
        new BodyPart 
        { 
            Name = "Nohy", 
            Type = BodyPartType.Leg, 
            Count = 8,
            Function = "Pohyb po stěnách, útok"
        },
        new BodyPart 
        { 
            Name = "Jedové čelisti", 
            Type = BodyPartType.Mouth, 
            Count = 1,
            Function = "Kousnutí s jedem",
            Attack = new BodyPartAttack
            {
                DamageDice = new Dice { Count = 1, Sides = DiceType.D8, Bonus = 0 },
                DamageType = DamageType.Poison,
                Initiative = 3
            },
            Appearance = "Ostré, kapající jedem"
        }
    }
};

// Počet očí
var eyes = giantSpiderAnatomy.BodyParts.First(p => p.Type == BodyPartType.Eye);
Console.WriteLine($"Obří pavouk má {eyes.Count} očí");  // 8
```

---

## Příklad 9: Použití v Race

```csharp
var dragonRace = new Race
{
    RaceId = 1,
    RaceName = "Červený drak",
    RaceDescription = "Mocný ohnivý drak z hor",
    
    BodyDimensins = new AnatomyProfile
    {
        RaceSize = RaceSize.E,
        WeightMin = 2000,
        WeightMax = 5000,
        BodyHeightMin = 500,
        BodyHeightMax = 1000,
        
        BodyParts = new List<BodyPart>
        {
            new BodyPart 
            { 
                Name = "Dračí hlava", 
                Type = BodyPartType.Head, 
                Count = 1,
                Attack = new BodyPartAttack
                {
                    DamageDice = new Dice { Count = 6, Sides = DiceType.D10, Bonus = 5 },
                    DamageType = DamageType.Fire
                }
            }
            // ... další části
        }
    }
};

// Přístup k anatomii z rasy
var dragonHead = dragonRace.BodyDimensins.BodyParts?.FirstOrDefault(p => p.Type == BodyPartType.Head);
if (dragonHead?.Attack != null)
{
    Console.WriteLine($"Drak útočí: {dragonHead.Attack.DamageDice.Count}d{(int)dragonHead.Attack.DamageDice.Sides}");
}
```

---

## Příklad 10: Prázdný profil (volitelné údaje)

```csharp
// Jednoduchá rasa bez detailů anatomie
var simpleRace = new AnatomyProfile
{
    RaceSize = RaceSize.B,
    WeightMin = 60,
    WeightMax = 90,
    BodyHeightMin = 160,
    BodyHeightMax = 190,
    
    BodyParts = null    // Žádné detailní informace!
};

// Bezpečná kontrola
bool hasDetailedAnatomy = simpleRace.BodyParts?.Any() ?? false;
Console.WriteLine($"Má detailní anatomii: {hasDetailedAnatomy}");  // False
```

---

## Shrnutí

### Kdy použít detailní anatomii (BodyParts):
- **Monstra** - pro popis útoků různými částmi těla
- **Zvířata** - pro speciální schopnosti (křídla, žábry, jedové žlázy)
- **Neobvyklé rasy** - hybridní bytosti, elementálové, podivná stvoření

### Kdy stačí základní profil:
- **Standardní humanoidní rasy** (lidé, elfové, trpaslíci)
- **Jednoduché NPC** bez bojových schopností
- **Běžná zvířata** bez speciálních útoků

### Výhody systému:
✅ Flexibilní - lze popsat cokoliv od člověka po hydru  
✅ Volitelné - `BodyParts` je nullable, nemusí být vyplněn  
✅ Rozšiřitelné - snadno přidat nové typy částí těla  
✅ Herní mechaniky - útočné/obranné hodnoty pro boj  
✅ Vizuální popis - `Appearance` pro narativní účely
