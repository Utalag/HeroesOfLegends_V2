Popis složky `HoL.Domain.ValueObjects`

Co sem patří:
- Hodnotové objekty (Value Objects) reprezentující doménní hodnoty bez identity.
  - Např. `AnatomyProfile`, `BodyPart`, `VulnerabilityProfil`, `Stat`, `ValueRange`, `Currency`, `Weapon`, `FightingSpirit`.
- Struktury, které mají význam v doméně a nesou data/nechovají stav persistence.
- Komplexní typy používané entitami jako součást stavu (např. `Race.BodyDimensions`).

Co sem nepatří:
- Repozitáře, DB kontexty, služby infrastruktury (patří do Infrastructure).
- Helper/utility kód, který je čistě implementační (patří do `HoL.Domain.Helpers`).

Doporučení:
- ValueObjects preferovat jako immutable (použít `record` nebo private settery s konstruktorem) pokud to kompatibilita s EF dovolí.
- Implementovat hodnotovou rovnost (`Equals`/`GetHashCode`) nebo použít `record`.
- Namespace: `HoL.Domain.ValueObjects` nebo vícekrokové podnamespace jako `HoL.Domain.ValueObjects.Anatomy` pro organizaci.

Příklad typických souborů:
- `AnatomyProfile.cs`
- `BodyPart.cs`
- `VulnerabilityProfil.cs`
- `Stat.cs`
- `ValueRange.cs`
- `Currency.cs`