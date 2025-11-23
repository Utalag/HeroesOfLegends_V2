# Stat - Příklady použití

## Základní přehled

Třída `Stat` reprezentuje herní statistiku s podporou úprav a bonusů. Obsahuje:
- `RawValue` - základní hodnota (např. hozená kostkami)
- `ValueAdjustment` - úprava hodnoty (např. rasový bonus)
- `BonusAdjustment` - úprava bonusu (např. magické předměty)
- `FinalValue` - výsledná hodnota po úpravách
- `FinalBonus` - výsledný bonus po úpravách

## StatModifierTable - Tabulka modifikátorů

```
Hodnota   | Bonus
----------|-------
1         | -5
2-3       | -4
4-5       | -3
6-7       | -2
8-9       | -1
10-12     | 0
13-14     | +1
15-16     | +2
17-18     | +3
19-20     | +4
21-22     | +5
...
```

---

## Příklad 1: Základní vytvoření statistiky

```csharp
using HeroesOfLegends.DataLayer.ValueObjects;

// Vytvoření základní statistiky Síly
var strength = new Stat
{
    Type = StatType.Strength,
    RawValue = 15
};

Console.WriteLine($"Raw Value: {strength.RawValue}");        // 15
Console.WriteLine($"Raw Bonus: {strength.RawBonus}");        // +2 (z tabulky)
Console.WriteLine($"Final Value: {strength.FinalValue}");    // 15
Console.WriteLine($"Final Bonus: {strength.FinalBonus}");    // +2
```

---

## Příklad 2: Statistika s rasovou úpravou

```csharp
// Trpaslík má +2 k Constitution
var constitution = new Stat
{
    Type = StatType.Constitution,
    RawValue = 14,              // Hozeno kostkami
    ValueAdjustment = 2         // Rasový bonus trpaslíka
};

Console.WriteLine($"Raw Value: {constitution.RawValue}");           // 14
Console.WriteLine($"Value Adjustment: {constitution.ValueAdjustment}"); // +2
Console.WriteLine($"Final Value: {constitution.FinalValue}");       // 16 (14 + 2)
Console.WriteLine($"Raw Bonus: {constitution.RawBonus}");           // +1 (z 14)
Console.WriteLine($"Final Bonus: {constitution.FinalBonus}");       // +2 (z 16)
```

**Vysvětlení:**
- RawValue = 14 → RawBonus = +1 (z tabulky)
- Final Value = 14 + 2 = 16 → Final Bonus = +2 (z tabulky)

---

## Příklad 3: Magický předmět zvyšující bonus

```csharp
// Kouzelník s Intelligence 16 + magický prsten +1 k bonusu
var intelligence = new Stat
{
    Type = StatType.Intelligence,
    RawValue = 16,
    BonusAdjustment = 1    // Prsten moudrosti
};

Console.WriteLine($"Raw Value: {intelligence.RawValue}");           // 16
Console.WriteLine($"Final Value: {intelligence.FinalValue}");       // 16
Console.WriteLine($"Raw Bonus: {intelligence.RawBonus}");           // +2 (z 16)
Console.WriteLine($"Final Bonus: {intelligence.FinalBonus}");       // +3 (+2 z tabulky +1 z prstenu)
```

**Vysvětlení:**
- Hodnota zůstává 16 (žádný ValueAdjustment)
- Bonus je +2 (z tabulky) + 1 (z prstenu) = +3

---

## Příklad 4: Kombinace všech úprav

```csharp
// Elf s Agility zvýšenou rasou i magickým plášťem
var agility = new Stat
{
    Type = StatType.Agility,
    RawValue = 15,              // Hozeno kostkami
    ValueAdjustment = 2,        // Elfí bonus
    BonusAdjustment = 1         // Plášť stínů
};

Console.WriteLine($"Raw Value: {agility.RawValue}");                // 15
Console.WriteLine($"Value Adjustment: {agility.ValueAdjustment}");  // +2
Console.WriteLine($"Bonus Adjustment: {agility.BonusAdjustment}");  // +1
Console.WriteLine($"Final Value: {agility.FinalValue}");            // 17 (15 + 2)
Console.WriteLine($"Raw Bonus: {agility.RawBonus}");                // +2 (z 15)
Console.WriteLine($"Final Bonus: {agility.FinalBonus}");            // +4 (z 17 = +3, +1 z pláště)
```

**Krok za krokem:**
1. RawValue = 15 → RawBonus = +2
2. ValueAdjustment = +2 → FinalValue = 17
3. FinalValue = 17 → bonus z tabulky = +3
4. BonusAdjustment = +1 → FinalBonus = +3 + 1 = +4

---

## Příklad 5: Nízká statistika s negativním bonusem

```csharp
// Slabý goblin se Strength 6
var weakStrength = new Stat
{
    Type = StatType.Strength,
    RawValue = 6
};

Console.WriteLine($"Raw Value: {weakStrength.RawValue}");      // 6
Console.WriteLine($"Raw Bonus: {weakStrength.RawBonus}");      // -2 (z tabulky)
Console.WriteLine($"Final Value: {weakStrength.FinalValue}");  // 6
Console.WriteLine($"Final Bonus: {weakStrength.FinalBonus}");  // -2
```

---

## Příklad 6: Použití v Dictionary (Race.StatsPrimar)

```csharp
// Vytvoření statistik pro rasu
var humanStats = new Dictionary<StatType, Stat>
{
    {
        StatType.Strength,
        new Stat { Type = StatType.Strength, RawValue = 10 }
    },
    {
        StatType.Agility,
        new Stat { Type = StatType.Agility, RawValue = 10 }
    },
    {
        StatType.Constitution,
        new Stat { Type = StatType.Constitution, RawValue = 10 }
    },
    {
        StatType.Intelligence,
        new Stat { Type = StatType.Intelligence, RawValue = 10 }
    },
    {
        StatType.Charisma,
        new Stat { Type = StatType.Charisma, RawValue = 10 }
    }
};

// Přístup ke statistice
var str = humanStats[StatType.Strength];
Console.WriteLine($"Human Strength: {str.FinalValue} (bonus: {str.FinalBonus})");
```

---

## Příklad 7: Dynamická změna statistiky

```csharp
var charisma = new Stat
{
    Type = StatType.Charisma,
    RawValue = 12
};

Console.WriteLine($"Initial - Value: {charisma.FinalValue}, Bonus: {charisma.FinalBonus}");
// Value: 12, Bonus: 0

// Postava si nasadí korunu vládce (+2 k Charisma)
charisma.ValueAdjustment = 2;
Console.WriteLine($"With Crown - Value: {charisma.FinalValue}, Bonus: {charisma.FinalBonus}");
// Value: 14, Bonus: +1

// Postava získá požehnání boha (+1 k bonusu)
charisma.BonusAdjustment = 1;
Console.WriteLine($"With Blessing - Value: {charisma.FinalValue}, Bonus: {charisma.FinalBonus}");
// Value: 14, Bonus: +2 (+1 z tabulky +1 z požehnání)
```

---

## Příklad 8: IStatRead a IStatWrite použití

```csharp
// IStatWrite - plný přístup (pro vytváření a editaci)
IStatWrite editableStat = new Stat
{
    Type = StatType.Strength,
    RawValue = 15,
    ValueAdjustment = 2,
    BonusAdjustment = 0
};

// IStatRead - pouze čtení (pro herní mechaniky)
IStatRead readOnlyStat = editableStat as IStatRead;

// UI může číst, ale ne měnit
Console.WriteLine($"Final Value: {readOnlyStat.FinalValue}");
Console.WriteLine($"Final Bonus: {readOnlyStat.FinalBonus}");

// readOnlyStat.RawValue = 20; // Chyba - není setter v IStatRead
```

---

## Reálný příklad: Výpočet útoku

```csharp
// Bojovník útočí mečem
var strengthStat = new Stat
{
    Type = StatType.Strength,
    RawValue = 18,              // Silný bojovník
    ValueAdjustment = 0,
    BonusAdjustment = 2         // Magický meč +2
};

int attackRoll = RollD20();                    // Např. 15
int attackBonus = strengthStat.FinalBonus;     // +5 (+3 z tabulky +2 z meče)
int totalAttack = attackRoll + attackBonus;    // 15 + 5 = 20

Console.WriteLine($"Attack roll: {attackRoll} + {attackBonus} = {totalAttack}");

int RollD20() => new Random().Next(1, 21);
```

---

## Shrnutí

- **RawValue** = základní hodnota (hod kostkami, fixed hodnota rasy)
- **ValueAdjustment** = rasové/profesní úpravy hodnoty
- **BonusAdjustment** = magické předměty, buffs, temporary efekty
- **FinalValue** = RawValue + ValueAdjustment
- **FinalBonus** = StatModifierTable.GetModifier(FinalValue) + BonusAdjustment

**Použití:**
- Při vytváření postavy nastavte `RawValue` (hod kostkami)
- Rasový bonus přidejte do `ValueAdjustment`
- Předměty a efekty do `BonusAdjustment`
- Pro herní mechaniky používejte `FinalValue` a `FinalBonus`
