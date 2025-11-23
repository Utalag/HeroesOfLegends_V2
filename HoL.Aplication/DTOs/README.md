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
- `WeaponDto.cs` - zbraň
- `FightingSpiritDto.cs` - bojový duch
- `VulnerabilityProfilDto.cs` - profil zranitelnosti

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

## Mapování

Mapování mezi Domain objekty a DTOs je definováno v `HoL.Aplication.MyMapper.Mapper` pomocí AutoMapper.

## Enumy

Enumy z Domain vrstvy (např. `RaceCategory`, `StatType`, `DiceType`, `VulnerabilityType`, atd.) se propagují přímo do Application vrstvy a jsou používány v DTOs.

## Doporučení

- DTOs by měly být jednoduché datové struktury bez business logiky.
- Pro každý nový doménový objekt vytvořte odpovídající DTO a přidejte mapování do Mapper.cs.
- Udržujte konzistentní pojmenování: pokud máte `DomainObject`, vytvořte `DomainObjectDto`.
- DTOs by neměly odkazovat přímo na doménové objekty - používejte pouze primitivní typy, enumy a jiné DTOs.
