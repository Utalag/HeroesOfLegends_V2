# HoL.Aplication DTOs Structure

Tato složka obsahuje Data Transfer Objects (DTOs) pro komunikaci mezi Application a dalšími vrstvami. DTOs jsou oddělené od doménových objektů a poskytují čistou separaci mezi vrstvami.

## Struktura složek

### EntitiDtos/
Obsahuje DTOs pro doménové entity (hlavní objekty s identitou).
- `RaceDto.cs` - DTO pro entitu Race

### ValueObjectDtos/
Obsahuje DTOs pro hodnot objekty (value objects) bez identity.
- `CurrencyDto.cs` - měnový systém (Gold, Silver, Copper)
- `SpecialAbilitiesDto.cs` - speciální schopnosti
- `TreasureDto.cs` - poklad (měny a jejich množství)
- `WeaponDto.cs` - zbraň
- `FightingSpiritDto.cs` - bojový duch

### AnatomiDtos/
Obsahuje DTOs pro anatomické struktury.
- `AnatomyProfileDto.cs` - profil anatomie (tělesné rozměry)
- `BodyPartDto.cs` - část těla
- `BodyPartAttackDto.cs` - útočné schopnosti části těla
- `BodyPartDefenseDto.cs` - obranné vlastnosti části těla
- `DiceDto.cs` - reprezentace hodu kostkou

### StatDtos/
Obsahuje DTOs pro statistiky a hodnoty.
- `StatDto.cs` - základní statistika
- `ValueRangeDto.cs` - rozsah hodnot (min/max s kostkami)

## Validátoři

Každý DTO má odpovídající validátor v složce `Validators/` stejné struktury:

```
HoL.Aplication/
├── DTOs/
│   ├── EntitiDtos/RaceDto.cs
│   ├── ValueObjectDtos/CurrencyDto.cs
│   └── ...
└── Validators/
    ├── EntitiValidators/RaceDtoValidator.cs
    ├── ValueObjectValidators/CurrencyDtoValidator.cs
    └── ...
```

**Validátoři používají FluentValidation** a zajišťují korektnost dat před zpracováním.

### Registrace validátorů v DI

Všechny validátory se registrují v Dependency Injection kontejneru:

```csharp
// Program.cs
services.AddValidatorsFromAssemblyContaining<RaceDtoValidator>();
```

## Mapování (AutoMapper)

Mapování probíhá ve dvou směrech:

### 1. Infrastructure → Domain (DbModel → Domain)
**Soubor:** `HoL.Infrastructure/Data/MapModels/DomainInfrastructureMapper.cs`
```
RaceDbModel → Race (via RaceDbModelToRaceConverter)
CurrencyGroupDbModel → CurrencyGroup
SingleCurrencyDbModel → SingleCurrency
TreasureDbModel → Treasure
```

### 2. Domain → DTO (Application responses)
**Soubor:** `HoL.Aplication/Mappers/ApplicationMapper.cs` (v procesu)
```
Race → RaceDto
CurrencyGroup → CurrencyGroupDto
SingleCurrency → CurrencyDto
Treasure → TreasureDto
SpecialAbilities → SpecialAbilitiesDto
BodyPart → BodyPartDto
```

### 3. DTO → Domain (Application requests)
**Soubor:** `HoL.Aplication/Mappers/ApplicationMapper.cs` (v procesu)
```
RaceDto → Race (validace pomocí RaceBuilder)
CurrencyDto → SingleCurrency
SpecialAbilitiesDto → SpecialAbilities
```

## Enumy

Enumy z Domain vrstvy jsou propagovány do Application vrstvy a DTOs:
- `RaceCategory` - kategorie rasy (Humanoid, Beast, atd.)
- `StatType` - typ statistiky (Strength, Dexterity, atd.)
- `DiceType` - typ kostky (D4, D6, D8, atd.)
- `VulnerabilityType` - typ zranitelnosti
- `BodyPartType` - typ části těla
- `MobilityType` - typ mobility (Walking, Flying, atd.)
- `ConvictionType` - přesvědčení (Good, Neutral, Evil)

## Architektura - DTO a mapování toku

```
┌─ REQUEST FLOW ─────────────────────────────┐
│                                            │
│ API Controller                             │
│ ↓ (JSON request body)                      │
│ RaceDto (with [ApiController] binding)     │
│ ↓ (FluentValidation)                       │
│ Validator.ValidateAsync(raceDto)           │
│ ↓ (if valid)                               │
│ ApplicationMapper (DTO → Domain)           │
│ Race domain object                         │
│ ↓                                          │
│ CQRS Command Handler / Service             │
│ Domain logic                               │
│ ↓                                          │
└────────────────────────────────────────────┘

┌─ RESPONSE FLOW ────────────────────────────┐
│                                            │
│ CQRS Query Handler / Service               │
│ ↓                                          │
│ IRaceRepository.GetByIdAsync(id)           │
│ ↓ (returns Race domain object)             │
│ RaceDbRepository                           │
│ ├─ DomainInfrastructureMapper              │
│ │  (DbModel → Domain via Converter)        │
│ └─ Returns Race                            │
│ ↓                                          │
│ ApplicationMapper (Domain → DTO)           │
│ ↓ (Race → RaceDto)                         │
│ RaceDto                                    │
│ ↓ (JSON serialization)                     │
│ API Response (JSON)                        │
│                                            │
└────────────────────────────────────────────┘
```

## Best Practices

- **DTOs by měly být jednoduché datové struktury bez business logiky**
  - Obsahují pouze properties s getters/setters
  - Bez metod vracejících berechnované hodnoty
  - Bez referencí na domain objekty

- **Pojmenování by mělo být konzistentní**
  - `DomainObject` → `DomainObjectDto`
  - Příklady: `Race` → `RaceDto`, `SpecialAbilities` → `SpecialAbilitiesDto`

- **Používejte pouze podporované typy v DTOs**
  - Primitivní typy: `string`, `int`, `double`, `bool`, `DateTime`, atd.
  - Enumy (přenesené z Domain)
  - Jiné DTOs
  - `List<T>` pro kolekce (ne `IEnumerable<T>`)
  - Nullable typy (`int?`, `string?`)

- **Každý DTO musí mít validátor**
  - Validátor se registruje v DI
  - Volá se před mapováním na domain objekt
  - Zajišťuje konzistenci dat

- **Mapování musí být oboustranné**
  - Domain → DTO: Pro API responses
  - DTO → Domain: Pro API requests (s validací)

- **Nepoužívejte veřejné konstruktory bez parametrů v domain objektech**
  - DTOs se mohou mapovat přes parameterless konstruktor
  - Domain objekty mají specifické konstruktory s validací

## Interfaces (IDtos)

Složka `Interfaces/IDtos/` obsahuje rozhraní pro specifické operace:
- `IStatDtos/` - rozhraní pro statistiky (Read/Write operace)

Tyto interfaces umožňují lepší abstrakci a testovatelnost při specifických požadavcích.

## Příklady mapování

### Domain → DTO (pro API responses)

```csharp
// V ApplicationMapper profilu:
CreateMap<Race, RaceDto>()
    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.RaceName))
    .ForMember(d => d.Category, opt => opt.MapFrom(s => s.RaceCategory))
    .ForMember(d => d.SpecialAbilities, opt => opt.MapFrom(s => s.SpecialAbilities))
    .ReverseMap(); // Umožňuje i DTO → Race mapování

// V aplikačním kódu:
var raceDto = _mapper.Map<RaceDto>(raceEntity);
```

### DTO → Domain (pro API requests)

```csharp
// Validace
var validationResult = await _validator.ValidateAsync(raceDto);
if (!validationResult.IsValid)
{
    throw new ValidationException(validationResult.Errors);
}

// Mapování na domain object
var race = _mapper.Map<Race>(raceDto);

// Pokud je složitější: Můžete použít RaceBuilder
var race = new RaceBuilder()
    .WithName(raceDto.Name)
    .WithCategory(raceDto.Category)
    // ...
    .Build();
```

## Status implementace

| Komponenta | Status | Poznámka |
|-----------|--------|---------|
| DTOs struktury | ✅ Hotovo | Všechny DTOs jsou vytvořeny |
| Validátoři | ✅ Hotovo | FluentValidation pro všechny DTOs |
| DomainInfrastructureMapper | ✅ Hotovo | DbModel → Domain mapování |
| ApplicationMapper | ⏳ V procesu | Domain ↔ DTO mapování |
| DI registrace | ⏳ V procesu | Bude v Program.cs |
| API vrstva | ⏳ Plánováno | Controllers s DTO binding |
