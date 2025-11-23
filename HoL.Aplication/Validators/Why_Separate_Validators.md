# Proč oddělit validátory? Výhody kompozice validátorů

## 🎯 Problém: Monolitický validátor

### Předtím (❌ Špatně):
```csharp
public class UpdatedRaceCommandValidator : AbstractValidator<UpdatedRaceCommand>
{
    public UpdatedRaceCommandValidator()
    {
        RuleFor(x => x.RaceDto).NotNull();
        
        // 300+ řádků validace přímo v commandu
        RuleFor(x => x.RaceDto.RaceName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RaceDto.Treasure.Gold).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RaceDto.FightingSpirit.DangerNumber).GreaterThanOrEqualTo(0);
        // ... 290+ dalších řádků
    }
}
```

**Problémy:**
- ❌ Duplicitní kód pokud validuješ `RaceDto` jinde
- ❌ Těžko se testuje (musíš testovat celý command)
- ❌ Nelze znovupoužít validaci `CurrencyDto` pro jiné entity
- ❌ Změna v `CurrencyDto` = hledání všech míst kde se validuje
- ❌ 300+ řádků v jednom souboru = nečitelné

## ✅ Řešení: Kompozice samostatných validátorů

### Nyní (✅ Správně):

#### 1. UpdatedRaceCommandValidator (15 řádků)
```csharp
public class UpdatedRaceCommandValidator : AbstractValidator<UpdatedRaceCommand>
{
    public UpdatedRaceCommandValidator()
    {
        RuleFor(x => x.RaceDto)
            .NotNull()
            .SetValidator(new RaceDtoValidator()!); // Deleguje na specializovaný validátor
    }
}
```

#### 2. RaceDtoValidator (EntitiValidators/)
```csharp
public class RaceDtoValidator : AbstractValidator<RaceDto>
{
    public RaceDtoValidator()
    {
        RuleFor(x => x.RaceName).NotEmpty().MaximumLength(100);
        
        // Deleguje na specializované validátory pro vnořené objekty
        RuleFor(x => x.Treasure)
            .NotNull()
            .SetValidator(new CurrencyDtoValidator()!);
        
        RuleFor(x => x.FightingSpirit)
            .NotNull()
            .SetValidator(new FightingSpiritDtoValidator()!);
        
        RuleForEach(x => x.SpecialAbilities)
            .SetValidator(new SpecialAbilitiesDtoValidator());
    }
}
```

#### 3. CurrencyDtoValidator (ValueObjectValidators/)
```csharp
public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
{
    public CurrencyDtoValidator()
    {
        RuleFor(x => x.Gold).GreaterThanOrEqualTo(0).When(x => x.Gold.HasValue);
        RuleFor(x => x.Silver).GreaterThanOrEqualTo(0).When(x => x.Silver.HasValue);
        RuleFor(x => x.Copper).GreaterThanOrEqualTo(0).When(x => x.Copper.HasValue);
        
        RuleFor(x => x)
            .Must(c => c.Gold.HasValue || c.Silver.HasValue || c.Copper.HasValue)
            .WithMessage("At least one currency must be specified");
    }
}
```

## 🚀 Výhody kompozice

### 1. **Znovupoužitelnost**
```csharp
// CurrencyDtoValidator můžeš použít kdekoli
public class CharacterDtoValidator : AbstractValidator<CharacterDto>
{
    public CharacterDtoValidator()
    {
        RuleFor(x => x.Inventory)
            .SetValidator(new CurrencyDtoValidator()!); // Stejný validátor!
    }
}

public class TreasureChestDtoValidator : AbstractValidator<TreasureChestDto>
{
    public TreasureChestDtoValidator()
    {
        RuleFor(x => x.Contents)
            .SetValidator(new CurrencyDtoValidator()!); // Opět stejný!
    }
}
```

### 2. **Snadné testování**
```csharp
[Fact]
public void Should_Validate_Currency_Independently()
{
    // Testuj jen CurrencyDto validaci, ne celý RaceDto
    var validator = new CurrencyDtoValidator();
    var currency = new CurrencyDto { Gold = -10 };
    
    var result = validator.TestValidate(currency);
    result.ShouldHaveValidationErrorFor(x => x.Gold);
}

[Fact]
public void Should_Validate_Race_With_Invalid_Currency()
{
    // RaceDtoValidator automaticky používá CurrencyDtoValidator
    var validator = new RaceDtoValidator();
    var race = new RaceDto 
    { 
        RaceName = "Elf",
        Treasure = new CurrencyDto { Gold = -10 } // nevalidní
    };
    
    var result = validator.TestValidate(race);
    result.ShouldHaveValidationErrorFor("Treasure.Gold"); // Automaticky!
}
```

### 3. **Maintainability - jedna změna, všude účinek**
```csharp
// Změníš CurrencyDtoValidator jednou...
public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
{
    public CurrencyDtoValidator()
    {
        // Nové pravidlo: maximální hodnota
        RuleFor(x => x.Gold)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(1_000_000) // <-- přidáno
            .When(x => x.Gold.HasValue);
    }
}

// ... a funguje automaticky všude kde se používá:
// - RaceDto
// - CharacterDto
// - TreasureChestDto
// - ShopItemDto
```

### 4. **Čitelnost - každý validátor má jednu zodpovědnost**
```
Validators/
├── EntitiValidators/
│   └── RaceDtoValidator.cs          (validuje Race-specific pravidla)
├── ValueObjectValidators/
│   ├── CurrencyDtoValidator.cs      (validuje pouze Currency)
│   ├── WeaponDtoValidator.cs        (validuje pouze Weapon)
│   └── FightingSpiritDtoValidator.cs (validuje pouze FightingSpirit)
```

## 📊 Srovnání velikosti kódu

| Přístup | Počet řádků | Znovupoužitelnost |
|---------|-------------|-------------------|
| ❌ Monolitický | 300+ v jednom souboru | 0% |
| ✅ Kompozice | 15-30 na validátor | 100% |

## 🔄 Jak to funguje v praxi

### Příklad: Validace RaceDto s vnořenými objekty

```csharp
var race = new RaceDto
{
    RaceName = "Elf",
    Treasure = new CurrencyDto { Gold = -10 }, // ❌ nevalidní
    FightingSpirit = new FightingSpiritDto { DangerNumber = 25 }, // ❌ nevalidní
    RaceWeapon = new WeaponDto { WeaponName = "" } // ❌ nevalidní
};

var validator = new RaceDtoValidator();
var result = validator.Validate(race);
```

**ValidationBehavior automaticky spustí:**
1. `RaceDtoValidator`
   - Validuje `RaceName` ✅
   - Zavolá `CurrencyDtoValidator` pro `Treasure`
     - ❌ Najde chybu: Gold < 0
   - Zavolá `FightingSpiritDtoValidator` pro `FightingSpirit`
     - ❌ Najde chybu: DangerNumber > 20
   - Zavolá `WeaponDtoValidator` pro `RaceWeapon`
     - ❌ Najde chybu: WeaponName je prázdný

**Výsledek:**
```json
{
  "errors": {
    "Treasure.Gold": ["Gold must be non-negative"],
    "FightingSpirit.DangerNumber": ["DangerNumber cannot exceed 20"],
    "RaceWeapon.WeaponName": ["Weapon name is required"]
  }
}
```

## 💡 Best Practices

### ✅ DO:
- Vytvoř samostatný validátor pro každý DTO
- Používej `SetValidator()` pro kompozici
- Drž validátory v odpovídající struktuře jako DTOs
- Testuj každý validátor samostatně

### ❌ DON'T:
- Nekopíruj validační pravidla mezi validátory
- Nevkládej validaci přímo do command validátoru
- Nevaliduj vnořené objekty inline (použij `SetValidator()`)

## 🧪 Testovací příklady

### Test samostatného validátoru
```csharp
[Theory]
[InlineData(-1, false)]
[InlineData(0, true)]
[InlineData(100, true)]
public void Should_Validate_Currency_Gold(int gold, bool shouldBeValid)
{
    var validator = new CurrencyDtoValidator();
    var currency = new CurrencyDto { Gold = gold };
    
    var result = validator.TestValidate(currency);
    
    if (shouldBeValid)
        result.ShouldNotHaveValidationErrorFor(x => x.Gold);
    else
        result.ShouldHaveValidationErrorFor(x => x.Gold);
}
```

### Test kompozice
```csharp
[Fact]
public void Should_Use_Nested_Validators()
{
    var validator = new RaceDtoValidator();
    var race = new RaceDto 
    { 
        RaceName = "Elf",
        Treasure = new CurrencyDto { Gold = -10 } // použije CurrencyDtoValidator
    };
    
    var result = validator.TestValidate(race);
    result.ShouldHaveValidationErrorFor("Treasure.Gold"); // Vnořená validace!
}
```

## 📈 Výsledek

**Před refaktoringem:**
- 1 soubor: `UpdatedRaceCommandValidator.cs` (300+ řádků)
- Znovupoužitelnost: 0%
- Testovatelnost: těžká

**Po refaktoringu:**
- 10+ souborů: každý 15-50 řádků
- Znovupoužitelnost: 100%
- Testovatelnost: snadná
- Maintainability: výborná

---

**Závěr:** Kompozice validátorů je **MNOHEM lepší** než monolitický přístup! 🎉
