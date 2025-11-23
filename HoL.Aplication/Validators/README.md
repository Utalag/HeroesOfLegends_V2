# HoL.Aplication Validators - Complete Structure

Kompletní struktura validátorů odpovídající DTOs struktuře. Každý validátor validuje **pouze jeden DTO typ** a může být **znovupoužit kdekoli**.

## 📁 Kompletní struktura

```
Validators/
├── EntitiValidators/              # Validátory pro entity (hlavní objekty s identitou)
│   └── RaceDtoValidator.cs        ✅ Vytvoř...
│
├── ValueObjectValidators/         # Validátory pro hodnotové objekty
│   ├── CurrencyDtoValidator.cs    ✅ Vytvořeno
│   ├── SpecialAbilitiesDtoValidator.cs  ✅ Vytvořeno
│   ├── WeaponDtoValidator.cs      ✅ Vytvořeno
│   ├── FightingSpiritDtoValidator.cs  ✅ Vytvořeno
│   └── VulnerabilityProfilDtoValidator.cs  ✅ Vytvořeno
│
├── AnatomiValidators/             # Validátory pro anatomické struktury
│   ├── AnatomyProfileDtoValidator.cs  ✅ Vytvořeno
│   ├── BodyPartDtoValidator.cs    ✅ Vytvořeno
│   ├── BodyPartAttackDtoValidator.cs  ✅ Vytvořeno
│   ├── BodyPartDefenseDtoValidator.cs  ✅ Vytvořeno
│   └── DiceDtoValidator.cs        ✅ Vytvořeno
│
└── StatValidators/                # Validátory pro statistiky
    ├── ValueRangeDtoValidator.cs  ✅ Vytvořeno
    └── StatDtoValidator.cs        ✅ Vytvořeno
```

## 📊 Přehled všech validátorů

### 1️⃣ EntitiValidators (1 validátor)

| Validátor | DTO | Popis | Znovupoužitelnost |
|-----------|-----|-------|-------------------|
| `RaceDtoValidator` | `RaceDto` | Kompletní validace rasy včetně kompozice s ostatními validátory | Race entity |

### 2️⃣ ValueObjectValidators (5 validátorů)

| Validátor | DTO | Popis | Znovupoužitelnost |
|-----------|-----|-------|-------------------|
| `CurrencyDtoValidator` | `CurrencyDto` | Validace měn (Gold/Silver/Copper >= 0, alespoň 1 zadaná) | Race, Character, Shop, Treasure |
| `SpecialAbilitiesDtoValidator` | `SpecialAbilitiesDto` | Validace schopností (název required, max délky) | Race, Character, Item, Spell |
| `WeaponDtoValidator` | `WeaponDto` | Validace zbraně (název required, max 100 chars) | Race, Character, Equipment |
| `FightingSpiritDtoValidator` | `FightingSpiritDto` | Validace bojového ducha (DangerNumber 0-20) | Race, NPC, Monster |
| `VulnerabilityProfilDtoValidator` | `VulnerabilityProfilDto` | Validace zranitelnosti (multipliers > 1, resistance 0-1) | Race, Monster, Character |

### 3️⃣ AnatomiValidators (5 validátorů)

| Validátor | DTO | Popis | Znovupoužitelnost |
|-----------|-----|-------|-------------------|
| `AnatomyProfileDtoValidator` | `AnatomyProfileDto` | Validace anatomie (rozměry, věk, části těla) | Race, Monster, NPC |
| `BodyPartDtoValidator` | `BodyPartDto` | Validace části těla (název, typ, počet, funkce) | Anatomy profile |
| `BodyPartAttackDtoValidator` | `BodyPartAttackDto` | Validace útoku části těla (damage, typ, iniciativa) | Body part s útokem |
| `BodyPartDefenseDtoValidator` | `BodyPartDefenseDto` | Validace obrany části těla (armor value 0-50) | Body part s obranou |
| `DiceDtoValidator` | `DiceDto` | Validace kostky (count 1-20, sides enum, bonus -50 až 100) | Damage, stats, random |

### 4️⃣ StatValidators (2 validátory)

| Validátor | DTO | Popis | Znovupoužitelnost |
|-----------|-----|-------|-------------------|
| `ValueRangeDtoValidator` | `ValueRangeDto` | Validace rozsahu hodnot (Min/Max s kostkami, konzistence) | Stats, HP, Damage ranges |
| `StatDtoValidator` | `StatDto` | Validace statistiky (raw/final value, adjustments, konzistence) | Character, Race stats |

## 🔗 Graf závislostí validátorů

```
RaceDtoValidator
├── CurrencyDtoValidator
├── SpecialAbilitiesDtoValidator
├── WeaponDtoValidator
├── FightingSpiritDtoValidator
├── VulnerabilityProfilDtoValidator
├── AnatomyProfileDtoValidator
│   └── BodyPartDtoValidator
│       ├── BodyPartAttackDtoValidator
│       │   └── DiceDtoValidator
│       └── BodyPartDefenseDtoValidator
└── ValueRangeDtoValidator (pro StatsPrimar)
```

## 💡 Příklady použití

### 1. Použití v Command Validatoru
```csharp
public class UpdatedRaceCommandValidator : AbstractValidator<UpdatedRaceCommand>
{
    public UpdatedRaceCommandValidator()
    {
        RuleFor(x => x.RaceDto)
            .NotNull()
            .SetValidator(new RaceDtoValidator()!);
    }
}
```

### 2. Znovupoužití CurrencyDtoValidator
```csharp
// V RaceDto
public class RaceDtoValidator : AbstractValidator<RaceDto>
{
    public RaceDtoValidator()
    {
        RuleFor(x => x.Treasure)
            .SetValidator(new CurrencyDtoValidator()!);
    }
}

// V CharacterDto (stejný validátor!)
public class CharacterDtoValidator : AbstractValidator<CharacterDto>
{
    public CharacterDtoValidator()
    {
        RuleFor(x => x.Inventory)
            .SetValidator(new CurrencyDtoValidator()!);
    }
}
```

### 3. Samostatné testování
```csharp
[Fact]
public void Should_Validate_Currency_Gold_Negative()
{
    var validator = new CurrencyDtoValidator();
    var currency = new CurrencyDto { Gold = -10 };
    
    var result = validator.TestValidate(currency);
    result.ShouldHaveValidationErrorFor(x => x.Gold);
}
```

## 📋 Validační pravidla summary

### Číselné rozsahy
- `BaseXP`: 0 - 100,000
- `ZSM`: 0 - 20
- `DomesticationValue`: 0 - 100
- `BaseInitiative`: 0 - 30
- `DangerNumber`: 0 - 20
- `ArmorValue`: 0 - 50
- `MaxAge`: 1 - 10,000
- `DiceCount`: 1 - 10 (stats), 1 - 20 (damage)
- `DiceBonus`: -50 až 100

### Délky stringů
- `RaceName`: max 100
- `RaceDescription`: max 1,000
- `RaceHistory`: max 5,000
- `AbilityName`: max 100
- `AbilityDescription`: max 500
- `WeaponName`: max 100
- `BodyPartName`: max 100
- `Function/Appearance`: max 200

### Min/Max konzistence
- `WeightMax > WeightMin`
- `BodyHeightMax > BodyHeightMin`
- `HeightMax > HeightMin`
- `ValueRange.Max > ValueRange.Min`
- `FinalValue = RawValue + ValueAdjustment`

### Custom business rules
- `BaseXP >= DomesticationValue * 5` (pro domesticable races)
- `Vulnerability multipliers > 1.0`
- `Resistance multipliers: 0 - 1`
- `ValueRange.Max = Min + DiceCount * (DiceType - 1)`
- Alespoň jedna měna musí být zadaná v `Currency`

## ✅ Checklist pro nový DTO

Když vytváříš nový DTO, přidej i validátor:

1. [ ] Vytvoř DTO v `DTOs/{Kategorie}/`
2. [ ] Vytvoř validator v `Validators/{Kategorie}Validators/`
3. [ ] Pojmenuj: `{NázevDto}Validator.cs`
4. [ ] Použij `SetValidator()` pro vnořené objekty
5. [ ] Přidej unit testy
6. [ ] Aktualizuj tuto tabulku

## 🧪 Testing Quick Reference

```csharp
// Setup
var validator = new CurrencyDtoValidator();
var dto = new CurrencyDto { Gold = -10 };

// Act
var result = validator.TestValidate(dto);

// Assert
result.ShouldHaveValidationErrorFor(x => x.Gold);
result.ShouldNotHaveValidationErrorFor(x => x.Silver);
```

## 📖 See Also

- `Why_Separate_Validators.md` - Důvody kompozice validátorů
- `../DTOs/README.md` - Struktura DTOs
- `../Behaviors/ValidationBehavior.cs` - MediatR pipeline integration
