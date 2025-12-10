# 📊 Mapa závislostí projektů - HeroesOfLegendsCQRS

## 🎯 Přehled projektu

```
HeroesOfLegendsCQRS/
├── HoL.Domain/           (Nezávislý)
├── HoL.Contracts/        (Reference: Domain)
├── HoL.Aplication/       (Reference: Domain, Contracts)
├── HoL.Infrastructure/   (Reference: Domain, Contracts)
└── HoL.API/              (Reference: Application, Infrastructure) [V plánu]
```

---

## 📈 Dependency Graph - Vizuální mapa

### Správná struktura - BEZ CYKLŮ ✅

```
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│                   🏗️ HoL.Domain                              │
│                 (NEZÁVISLÝ - baseline)                       │
│         • Entities, ValueObjects, Enums                      │
│         • Domain interfaces                                  │
│                                                              │
└──────┬──────────────────────────────────────────────────┬───┘
       │                                                  │
       │ (references)                                     │ (references)
       │                                                  │
       v                                                  v
┌──────────────────┐                          ┌──────────────────────┐
│ HoL.Contracts    │◄─────────────────────────│ HoL.Aplication       │
│                  │  (references) ✓          │                      │
│ • IGenericRepo   │                          │ • Handlers           │
│ • IRaceRepository│                          │ • Validators         │
│                  │  SPRÁVNĚ: Application    │ • DTOs               │
│ Reference:       │  → Contracts (abstrakce) │ • Behaviours         │
│ ✅ Domain        │                          │                      │
│ ❌ Application   │                          │ Reference:           │
│ ❌ Infrastructure│                          │ ✅ Domain            │
│                  │                          │ ✅ Contracts         │
│                  │                          │ ❌ Infrastructure    │
└────────┬─────────┘                          │ ❌ API               │
         │                                    │                      │
         │ (references) ✓                     └──────────────────────┘
         │                                           │
         │                                           │
         v                                           │
┌──────────────────────────┐                         │
│ HoL.Infrastructure       │◄───────────────────────-┘
│                          │  (references) ✓
│ • GenericRepository      │
│ • RaceRepository         │  SPRÁVNĚ: Infrastructure
│ • DbContext              │  → Contracts (implementuje rozhraní)
│ • Migrations             │
│                          │
│ Reference:               │
│ ✅ Domain                │
│ ✅ Contracts             │
│ ❌ Application           │
│ ❌ API                   │
└────────┬─────────────────┘
         │
         │ (implementuje)
         v
   💾 Database (SQL Server 9.0)
```

---

## 📋 Tabulka referencí projektů

### HoL.Domain

| Vlastnost | Hodnota |
|-----------|---------|
| **Target Framework** | .NET 9.0 |
| **Referencuje projekty** | ❌ Žádné |
| **NuGet dependencies** | • Microsoft.EntityFrameworkCore.Abstractions 9.0.0<br>• Microsoft.Extensions.Logging.Abstractions 10.0.0 |
| **Obsah** | Entities, ValueObjects, Enums, Interfaces |
| **Role** | Baseline - business domain |
| **Cyklus?** | ❌ Ne (bezpečné) |

### HoL.Contracts

| Vlastnost | Hodnota |
|-----------|---------|
| **Target Framework** | .NET 9.0 |
| **Referencuje projekty** | • HoL.Domain ✅ |
| **NuGet dependencies** | Žádné |
| **Obsah** | IGenericRepository, IRaceRepository |
| **Role** | Abstrakce - rozhraní |
| **Cyklus?** | ❌ Ne (bezpečné) |

### HoL.Aplication

| Vlastnost | Hodnota |
|-----------|---------|
| **Target Framework** | .NET 9.0 |
| **Referencuje projekty** | • HoL.Domain ✅<br>• HoL.Contracts ✅ |
| **NuGet dependencies** | • MediatR 14.0.0<br>• FluentValidation 12.1.1<br>• AutoMapper.Collection 12.0.0<br>• AutoMapper.Collection.EntityFrameworkCore 12.0.0<br>• AutoMapper.Extensions.EnumMapping 6.0.0<br>• Microsoft.AspNetCore.Http.Abstractions 2.3.0 |
| **Obsah** | Handlers (Commands/Queries), DTOs, Validators, Behaviours |
| **Role** | Application layer - use cases |
| **Cyklus?** | ❌ Ne (Application → Contracts ✓) |

### HoL.Infrastructure

| Vlastnost | Hodnota |
|-----------|---------|
| **Target Framework** | .NET 9.0 |
| **Referencuje projekty** | • HoL.Domain ✅<br>• HoL.Contracts ✅ |
| **NuGet dependencies** | • Microsoft.EntityFrameworkCore 9.0.0<br>• Microsoft.EntityFrameworkCore.SqlServer 9.0.0<br>• Microsoft.EntityFrameworkCore.Tools 9.0.0<br>• Microsoft.AspNetCore.Http.Abstractions 2.3.0 |
| **Obsah** | GenericRepository, RaceRepository, DbContext, Migrations |
| **Role** | Infrastructure layer - persistence |
| **Cyklus?** | ❌ Ne (Infrastructure → Contracts ✓) |

### HoL.API (plánováno)

| Vlastnost | Hodnota |
|-----------|---------|
| **Target Framework** | .NET 9.0 (plánováno) |
| **Referencuje projekty** | • HoL.Aplication ✅ (pro handlers)<br>• HoL.Infrastructure ✅ (pro DI) |
| **NuGet dependencies** | ASP.NET Core, Entity Framework Core |
| **Obsah** | Controllers, Program.cs (DI), appsettings.json |
| **Role** | Presentation layer - API endpoints |
| **Cyklus?** | ❌ Ne (API je entry point) |

---

## ✅ Kontrola cyklických závislostí

### ✅ BEZPEČNÉ referenční cesty:

```
Domain ← Contracts ← Application ← API
         ↑          ↑
         └─────────  Infrastructure
```

- ✅ Domain → nic
- ✅ Contracts → Domain
- ✅ Application → Domain + Contracts
- ✅ Infrastructure → Domain + Contracts
- ✅ API → Application + Infrastructure

### ❌ ZAKÁZANÉ referenční cesty:

```
❌ Application → Infrastructure (přímě!)
   └─ Řešení: Používej Contracts (abstrakce)

❌ Contracts → Infrastructure
   └─ Řešení: Contracts referencuje jen Domain

❌ Domain → cokoliv
   └─ Domain je vždy baseline
```

---

## 🔄 Správná komunikace mezi vrstvami

```
HTTP Request
    ↓
┌─────────────────────────────────────┐
│ API Controller                      │
│ inject IMediator, IRaceRepository   │
└────────────┬────────────────────────┘
             │
             │ _mediator.Send(command)
             v
┌─────────────────────────────────────┐
│ Application Handler                 │
│ inject IRaceRepository (z Contracts)│ ✅ Správně!
│ (NE: RaceRepository z Infrastructure)│ ❌ Špatně!
└────────────┬────────────────────────┘
             │
             │ await _repository.AddAsync()
             v
┌─────────────────────────────────────┐
│ Infrastructure Repository           │
│ implementuje IRaceRepository        │ ✅ Správně!
│ (GenericRepository : IRaceRepository)│
└────────────┬────────────────────────┘
             │
             │ DbContext.SaveChangesAsync()
             v
         💾 Database
```

---

## 📦 NuGet Package Dependencies

### Shared across projects:
- ✅ Microsoft.AspNetCore.Http.Abstractions (Application, Infrastructure)

### Application specific:
- MediatR 14.0.0 (CQRS mediator)
- FluentValidation 12.1.1 (input validation)
- AutoMapper* 12.0.0+ (DTO ↔ Entity mapping)

### Infrastructure specific:
- Microsoft.EntityFrameworkCore 9.0.0 (ORM)
- Microsoft.EntityFrameworkCore.SqlServer 9.0.0 (SQL Server provider)
- Microsoft.EntityFrameworkCore.Tools 9.0.0 (migrations)

### Domain specific:
- Microsoft.EntityFrameworkCore.Abstractions (for IEntity interface)
- Microsoft.Extensions.Logging.Abstractions (for ILogger)

---

## 🏗️ Fyzická struktura na disku k 11.12.2025

```
C:\...\HeroesOfLegendsCQRS\ 
│
├── HoL.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Enums/
│   ├── Interfaces/
│   ├── LogMessages/
│   ├── Examples/
│   └── HoL.Domain.csproj
│
├── HoL.Contracts/
│   ├── IGenericRepository.cs
│   ├── IRaceRepository.cs
│   ├── README.md
│   └── HoL.Contracts.csproj
│
├── HoL.Aplication/
│   ├── Handlers/
│   │   ├── Commands/
│   │   └── Queries/
│   ├── DTOs/
│   ├── Validators/
│   ├── Behaviours/
│   ├── MyMapper.cs
│   └── HoL.Aplication.csproj
│
├── HoL.Infrastructure/
│   ├── Data/
│   │   ├── SqlDbContext.cs
│   │   ├── ModelConfigurations/
│   │   └── Migrations/
│   ├── Repositories/
│   │   ├── GenericRepository.cs
│   │   └── RaceRepository.cs
│   ├── Logging/
│   │   ├── RepositoryLog.cs
│   │   └── LogIdFactory.cs
│   └── HoL.Infrastructure.csproj
│
├── HoL.API/  (plánováno)
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
│   └── HoL.API.csproj
│
└── HeroesOfLegendsCQRS.sln
```

---

## 🔍 Analýza bezpečnosti závislostí

### ✅ Aktuální stav: BEZPEČNÝ

**Co je dobře:**
- ✅ Domain je nezávislý (baseline)
- ✅ Contracts reference jen Domain
- ✅ Application reference Domain + Contracts (ne Infrastructure!)
- ✅ Infrastructure reference Domain + Contracts
- ✅ Žádné cyklické závislosti (circular references)

**Build status:** ✅ Úspěšný - žádné chyby!

### 🚨 Možná rizika:

| Riziko | Stav | Řešení |
|--------|------|--------|
| Application → Infrastructure | ❌ Není | Pokud by byl, odstranit import |
| Contracts → Infrastructure | ❌ Není | Pokud by byl, odstranit import |
| Cyklus Domain ← → Contracts | ❌ Není | Kontrolovat v .csproj |
| Missing repository DI | ✅ Má | Program.cs zaregistruje: `services.AddScoped<IRaceRepository, RaceRepository>();` |

---

## 📝 Poznámka k správě závislostí

### Best practices:

1. **Při přidávání nového projektu:**
   - Ověř, že neporušuješ vrstvový model
   - Přidej `ProjectReference` jen na "nižší" vrstvy
   - Nikdy nevytvářej bidirectional reference

2. **Při přidávání nového NuGet balíčku:**
   - Domain: jen abstractions (Microsoft.*.Abstractions)
   - Application: validators, mappers, CQRS (MediatR, FluentValidation, AutoMapper)
   - Infrastructure: ORM, persistence (EF Core, SQL Server)

3. **Při používání tříd z jiného projektu:**
   - Vždy preferuj rozhraní (Interface) před konkrétní třídou
   - Pokud nemůžeš přistupovat k rozhraní, vrstva je špatně postavena

---

<div style="text-align: center; color: #888; margin-top: 2em;">
  <small>📊 Project Dependencies Map | Verze 1.0 | Last Updated: 2025-12-06 | Build Status: ✅ SUCCESS</small>
</div>
