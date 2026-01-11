# Heroes of Legends - CQRS & Clean Architecture

![.NET 9](https://img.shields.io/badge/.NET-9.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-In%20Development-yellow)

Projekt **Heroes of Legends** je komplexní aplikace postavená na architektuře **Clean Architecture** a **CQRS** (Command Query Responsibility Segregation) s využitím **Entity Framework Core 9** a **SQL Server**.

## 📋 Obsah

- [Přehled projektu](#přehled-projektu)
- [Technologie](#technologie)
- [Architektura](#architektura)
- [Struktura projektu](#struktura-projektu)
- [Funkčnost](#funkčnost)
- [Stavový přehled](#stavový-přehled)
- [Začínáme](#začínáme)
- [Testování](#testování)
- [Dokumentace](#dokumentace)

---

## 📖 Přehled projektu

**Heroes of Legends** je RPG engine pro správu ras, postav a jejich vlastností. Projekt implementuje komplexní doménový model pro:

- 🧝 **Rasy (Races)** - Různé rasy postav (Elf, Dragon, Humanoid, atd.)
- 💰 **Měnový systém** - Gold, Silver, Copper s poklady
- ⚔️ **Anatomii postav** - Tělesné rozměry, части těla, útok/obrana
- 📊 **Statistiku** - Síla, Inteligence, Charisma, atd. s kostkami
- 🛡️ **Zranitelnosti a Mobility** - Resistance, weakness, flying, atd.
- ✨ **Speciální schopnosti** - Custom abilities pro každou rasu
- 🏛️ **Hierarchii ras** - Sociální struktury a kastovní systémy

---

## 🛠️ Technologie

### Backend Framework
- **.NET 9** - Nejnovější verze .NET frameworku
- **C# 13.0** - Moderní jazyk s LINQ, pattern matching a record types

### Data Access & Persistence
- **Entity Framework Core 9** - ORM pro .NET
- **SQL Server** - Relační databáze
- **Migrations** - Databázová schémata a verzování

### Architektonické vzory
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - Abstrakce persistence logiky
- **Builder Pattern** - Fluent API pro tvorbu komplexních objektů
- **Dependency Injection** - IoC kontejner pro loose coupling

### Testing & Quality
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework
- **FluentValidation** - Validace vstupů

### Mapování
- **AutoMapper** - DTO ↔ Domain mapování
- **Custom Converters** - Specifická mapování s logikou

---

## 🏗️ Architektura

### Clean Architecture - Vrstvy

```
┌─────────────────────────────────────────┐
│  Presentation Layer                     │
│  (Controllers, API Endpoints)           │
│  ⏳ Plánováno                           │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│  Application Layer                      │
│  ├─ Handlers (CQRS)                     │
│  ├─ Services                            │
│  ├─ DTOs & Validators (FluentValidation)│
│  └─ AutoMapper Profiles                 │
│  🔄 V Procesu                           │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│  Domain Layer                           │
│  ├─ Entities (Race, SingleCurrency)     │
│  ├─ Value Objects (Treasure, BodyPart)  │
│  ├─ Domain Logic (RaceBuilder, Helpers) │
│  └─ Interfaces                          │
│  ✅ Hotovo                              │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│  Infrastructure Layer                   │
│  ├─ Repositories                        │
│  ├─ DbContext & Models                  │
│  ├─ Data Access (EF Core)               │
│  └─ Logging                             │
│  ✅ Hotovo (80%)                        │
└─────────────────────────────────────────┘
```

### Datový tok (DataFlow)

```
HTTP Request (JSON)
    ↓
┌──────────────────────────┐
│ API Controller           │
│ (DTO Binding)            │
└────────┬─────────────────┘
         │ Validace (FluentValidation)
         ↓
┌──────────────────────────┐
│ CQRS Handler/Service     │
│ (ApplicationMapper)       │
└────────┬─────────────────┘
         │ DTO → Domain Entity
         ↓
┌──────────────────────────┐
│ Domain Business Logic    │
│ (RaceBuilder, Rules)     │
└────────┬─────────────────┘
         │
         ↓
┌──────────────────────────┐
│ Repository               │
│ (Domain → DbModel)       │
└────────┬─────────────────┘
         │
         ↓
┌──────────────────────────┐
│ EF Core + SQL Server     │
│ Persistence              │
└──────────────────────────┘
```

---

## 📂 Struktura projektu

```
HeroesOfLegendsCQRS/
├── HoL.Domain/                      # 🔵 Domain Layer
│   ├── Entities/
│   │   ├── Race.cs                  # Hlavní entita rasy
│   │   └── CurrencyEntities/
│   │       ├── SingleCurrency.cs
│   │       └── CurrencyGroup.cs
│   ├── ValueObjects/
│   │   ├── Treasure.cs
│   │   ├── SpecialAbilities.cs
│   │   ├── AnatomiObjects/          # Tělesná anatomie
│   │   │   ├── BodyDimension.cs
│   │   │   ├── BodyPart.cs
│   │   │   └── Stat.cs
│   │   └── README.md
│   ├── Builders/
│   │   └── RaceBuilder.cs           # Fluent API pro Race
│   ├── Enums/                       # Domény enumy
│   │   ├── RaceCategory.cs
│   │   ├── BodyStat.cs
│   │   ├── DiceType.cs
│   │   └── ...
│   └── Interfaces/
│       └── Read/Write interfaces
│
├── HoL.Infrastructure/              # 🟢 Infrastructure Layer
│   ├── Data/
│   │   ├── SqlDbContext.cs          # EF Core DbContext
│   │   ├── Models/
│   │   │   ├── RaceDbModel.cs
│   │   │   └── ...
│   │   ├── Configuration/           # EF Core Fluent API
│   │   └── Migrations/              # DB Migrations
│   ├── Repositories/
│   │   ├── RaceDbRepository.cs      # ✅ CRUD + Paging
│   │   ├── SingleCurrencyRepository.cs
│   │   ├── CurrencyGroupRepository.cs
│   │   └── ReadMe-Repository.md
│   ├── Mappers/
│   │   ├── DomainInfrastructureMapper.cs
│   │   └── Converters/              # Custom mapovací logika
│   └── Logging/
│       └── RepositoryLog.cs
│
├── HoL.Aplication/                  # 🟡 Application Layer
│   ├── DTOs/                        # Data Transfer Objects
│   │   ├── EntitiDtos/
│   │   │   └── RaceDto.cs
│   │   ├── ValueObjectDtos/
│   │   │   ├── CurrencyDto.cs
│   │   │   ├── SpecialAbilitiesDto.cs
│   │   │   └── TreasureDto.cs
│   │   ├── AnatomiDtos/
│   │   ├── StatDtos/
│   │   └── README.md
│   ├── Validators/                  # FluentValidation
│   │   ├── EntitiValidators/
│   │   ├── ValueObjectValidators/
│   │   └── README.md
│   ├── Handlers/                    # ⏳ CQRS Handlers
│   │   ├── Commands/
│   │   └── Queries/
│   └── Mappers/                     # ⏳ ApplicationMapper
│
├── HoL.Contracts/                   # 🔷 Contracts (Interfaces)
│   ├── Repositories/
│   │   └── IRaceRepository.cs
│   └── Services/
│
├── HoL.InfrastructureTest/          # 🧪 Infrastructure Tests
│   ├── Mapping/
│   │   ├── RaceDbModelToRaceMappingTest.cs  (10 testů)
│   │   ├── CurrencyGroupMappingTest.cs      (4 testy)
│   │   └── TreasureMappingTest.cs           (6 testů)
│   ├── Repositories/
│   │   └── SingleCurrencyRepoTest.cs        (16 testů)
│   └── ArrangeData/
│       └── ArrangeClass.cs          # Testovací data
│
├── HoL.DomainTest/                  # 🧪 Domain Tests
│   └── Builder/
│       └── RaceBuilderTest.cs
│
└── TODO.md                          # 📋 Projekt plán
```

---

## ✨ Funkčnost

### ✅ Implementováno

#### Domain Layer
- ✅ Race entita s komplexní strukturou
- ✅ SingleCurrency & CurrencyGroup entities
- ✅ Value Objects (Treasure, BodyPart, SpecialAbilities, atd.)
- ✅ RaceBuilder s fluent API
- ✅ Comprehensive enum system
- ✅ Domain interfaces pro čtení/zápis

#### Infrastructure Layer
- ✅ EF Core 9 konfiguraci (Fluent API)
- ✅ SQL Server databázová schémata
- ✅ GenericRepository base class
- ✅ RaceDbRepository s CRUD + paging
- ✅ SingleCurrencyRepository s CRUD + paging
- ✅ CurrencyGroupRepository
- ✅ TreasureRepository
- ✅ AutoMapper DomainInfrastructureMapper
- ✅ Logging s performance tracking (Stopwatch)
- ✅ Eager Loading & relationship management

#### Application Layer (Částečně)
- ✅ DTO struktury pro všechny domain objekty
- ✅ FluentValidation validátory
- ✅ DTO Readme dokumentace

#### Testing
- ✅ 20 Mapping testů (RaceDbModel → Race, CurrencyGroup, Treasure)
- ✅ 16 Repository testů (SingleCurrency CRUD + Paging)
- ✅ TestDbFixture s in-memory databází
- ✅ ArrangeClass s testovacími daty (Elf, Red Dragon)
- ✅ Unit testy pro RaceBuilder

### 🔄 V Procesu

- 🔄 AutoMapper mapování Domain ↔ DTO
- 🔄 Dependency Injection registrace
- 🔄 CQRS Handlers implementace
- 🔄 RaceDbRepository testy
- 🔄 JSON deserializace pro SpecialAbilities

### ⏳ Plánováno

- ⏳ API vrstva (Controllers)
- ⏳ CQRS Commands & Queries
- ⏳ Integration testy
- ⏳ Další entity repository testy
- ⏳ API dokumentace (Swagger)
- ⏳ Performance optimalizace

---

## 🚀 Stavový přehled

### Statistika kódu

| Vrstva | Status | Pokrytí |
|--------|--------|---------|
| **Domain** | ✅ Hotovo | 100% |
| **Infrastructure** | ✅ Hotovo | 80% |
| **Application** | 🔄 V procesu | 50% |
| **Presentation** | ⏳ Plánováno | 0% |
| **Testování** | ✅ Hotovo | 20+ testů |

### Testy

```
Total Tests: 40+
├── Infrastructure Tests
│   ├── Mapping Tests: 20 ✅
│   └── Repository Tests: 16 ✅
└── Domain Tests
    └── Builder Tests: 5+ ✅
```

### Kritické body (Bloaters)

1. **🔴 DI Registrace** - Bez toho se nic nepoužívá
2. **🔴 ApplicationMapper** - Domain ↔ DTO mapování
3. **🔴 JSON Deserializace** - SpecialAbilities `System.Text.Json` issue
4. **🟡 Async Refaktor** - MapToRace synchronní FirstOrDefault

Podrobnosti v [`TODO.md`](TODO.md)

---

## 🔧 Začínáme

### Předpoklady

- `.NET 9 SDK` nebo vyšší
- `SQL Server 2019+` nebo LocalDB
- Visual Studio 2022 / VS Code

### Instalace

1. **Klonování repozitáře**
   ```bash
   git clone https://github.com/Utalag/HeroesOfLegends_V2.git
   cd HeroesOfLegendsCQRS
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Databázové migrace**
   ```bash
   cd HoL.Infrastructure
   dotnet ef database update
   ```

4. **Build projektu**
   ```bash
   dotnet build
   ```

---

## 🧪 Testování

### Spuštění testů

```bash
# Všechny testy
dotnet test

# Pouze Infrastructure testy
dotnet test HoL.InfrastructureTest

# Pouze Domain testy
dotnet test HoL.DomainTest

# S detailním výstupem
dotnet test --logger "console;verbosity=detailed"
```

### Test Pokrytí

- **Mapping**: 20 testů ✅ (RaceDbModel → Race mapování)
- **Repository**: 16 testů ✅ (CRUD + Paging operace)
- **Builder**: 5+ testů ✅ (RaceBuilder fluent API)

Testovací data jsou v `ArrangeClass.cs` s konstantami pro:
- ✅ Elf (základní rasa)
- ✅ Red Dragon (komplikovaná rasa s pokladem)

---

## 📚 Dokumentace

### Interní Dokumentace

- **[TODO.md](TODO.md)** - Detailní plán projektu, priorita úkolů
- **[HoL.Domain/ValueObjects/README.md](HoL.Domain/ValueObjects/README.md)** - Domén objekty
- **[HoL.Infrastructure/Repositories/ReadMe-Repository.md](HoL.Infrastructure/Repositories/ReadMe-Repository.md)** - Repository Pattern & DataFlow
- **[HoL.Aplication/DTOs/README.md](HoL.Aplication/DTOs/README.md)** - DTO struktura & mapování
- **[HoL.Aplication/Validators/README.md](HoL.Aplication/Validators/README.md)** - Validační pravidla
- **[HoL.Contracts/README.md](HoL.Contracts/README.md)** - Interface kontrakty

### Klíčové koncepty

#### RaceBuilder - Fluent API

```csharp
var race = new RaceBuilder()
    .WithName("Elf")
    .WithCategory(RaceCategory.Humanoid)
    .WithBodyDimensions(new BodyDimension(RaceSize.B))
    .WithDescription("Elegantní a dlouhověcí elfové", "Starobylá rasa...")
    .WithBaseInitiative(5)
    .WithBaseXP(100)
    .AddBodyPart(new BodyPart("hlava", BodyPartType.Head, 1))
    .AddSpecialAbility(new SpecialAbilities("Dlouhovekost", "Žiji extrémně dlouho"))
    .Build();
```

#### Repository Pattern - Eager Loading

```csharp
// Automaticky loaduje Treasure + CurrencyGroup + Currencies
var race = await _raceRepository.GetByIdAsync(id);

// Stránkování + Řazení
var page = await _raceRepository.GetPageAsync(
    page: 1, 
    size: 10,
    sortBy: nameof(RaceDbModel.RaceName),
    direction: SortDirection.Ascending);
```

#### Mapování - Vícevrstvé

```
DTO → Domain Entity → DatabaseModel → SQL Server
(Validation) → (Business Logic) → (Persistence) → (Storage)
```

---

## 🏛️ Architektonické rozhodnutí

### 1. Clean Architecture
- Oddělení odpovědnosti do vrstev
- Dependency Injection
- Interface-based design

### 2. CQRS Pattern
- Oddělení čtení (Queries) a zápisu (Commands)
- Optimalizace read/write operací
- Scalability & Performance

### 3. Repository Pattern
- Abstrakce persistence logiky
- Testovatelnost
- Database independence

### 4. Builder Pattern
- Fluent API pro tvorbu komplexních objektů
- Čitelnost a údržovatelnost
- Immutability support

### 5. AutoMapper + Custom Converters
- DTO → Domain → DbModel mapování
- Custom logika pro komplexní objekty (RaceBuilder)
- Type-safe mapping

---

## 🐛 Známé problémy

### 1. JSON Deserializace SpecialAbilities
**Status:** 🔴 Kritické  
**Problém:** System.Text.Json chyba - parametry konstruktoru se neshodují s JSON vlastnostmi  
**Řešení:** Přidat `[JsonConstructor]` a `[JsonPropertyName]` atributy

### 2. MapToRace Synchronní volání
**Status:** 🟡 Střední  
**Problém:** FirstOrDefault() v async kontextu blokuje thread  
**Řešení:** Refaktor na FirstOrDefaultAsync()

### 3. DI Registrace
**Status:** 🔴 Kritické  
**Problém:** Nejsou zaregistrovány repositories v DI  
**Řešení:** Přidat extension metodu v Program.cs

Další v [`TODO.md`](TODO.md) - Sekcí "Známé problémy"

---

## 👥 Přispívání

Projekt je nyní v aktivním vývoji. Příspěvky jsou vítány!

1. Fork repositáře
2. Vytvořte feature branch (`git checkout -b feature/AmazingFeature`)
3. Commitujte změny (`git commit -m 'Add some AmazingFeature'`)
4. Pushujte do branche (`git push origin feature/AmazingFeature`)
5. Otevřete Pull Request

Prosím, sledujte prioritu v [`TODO.md`](TODO.md) - speciálně kritické body.

---

## 📋 Licenze

Tento projekt je licencován pod MIT licencí - viz [`LICENSE`](LICENSE) soubor pro detaily.

---

## 🔗 Užitečné zdroje

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern - Microsoft](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Entity Framework Core - Official Docs](https://docs.microsoft.com/en-us/ef/core/)
- [Repository Pattern - Microsoft](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [AutoMapper Documentation](https://automapper.org/)

---

## 📞 Kontakt

Pro otázky a návrhy otevřete GitHub issue nebo kontaktujte správce projektu.

---

**Poslední aktualizace:** `{{ TODAY }}`  
**Verze:** `0.5.0 (In Development)`  
**Status:** 🟡 Aktivní vývoj
