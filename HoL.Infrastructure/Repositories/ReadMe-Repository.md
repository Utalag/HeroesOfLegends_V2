# Repository Pattern - HoL.Infrastructure

Tato dokumentace popisuje implementaci Repository Pattern v HoL.Infrastructure vrstvě.

---

## 📊 Datový tok (DataFlow)

### Kompletní flow od requestu k databázi a zpět

```
┌─────────────────────────────────────────────────────────────┐
│ HTTP REQUEST (JSON)                                         │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ APPLICATION LAYER (Handler/Service)                         │
├─────────────────────────────────────────────────────────────┤
│ 1. Přijme: EntityDto                                        │
│ 2. Validuje: EntityDto (FluentValidation, DataAnnotations)  │
│ 3. Mapuje: EntityDto → DomainEntity (AutoMapper)            │
│ 4. Aplikuje: Business pravidla a logiku na DomainEntity     │
│ 5. Volá: repository.AddAsync(domainEntity)                  │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ INFRASTRUCTURE LAYER (Repository)                           │
├─────────────────────────────────────────────────────────────┤
│ 1. Přijme: DomainEntity (z Domain Layer)                    │
│ 2. Mapuje: DomainEntity → DatabaseModel (persistence)       │
│ 3. Uloží: do databáze přes ORM (EF Core)                    │
│ 4. SaveChangesAsync: Uložení do DB                          │
│ 5. Mapuje: DatabaseModel → DomainEntity (ID + audit fields) │
│ 6. Vrátí: DomainEntity nebo EntityId do Application Layer   │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ DATABASE LAYER (SQL Server)                                 │
├─────────────────────────────────────────────────────────────┤
│ Persistence v SQL Server                                    │
│ EF Core change tracking                                     │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ QUERY FLOW - Čtení dat                                      │
├─────────────────────────────────────────────────────────────┤
│ 1. Application volá: repository.GetByIdAsync(id)            │
│ 2. Repository: dbSet.Include(...).FirstOrDefaultAsync(...)  │
│ 3. EF Core: SQL query s eager loading                       │
│ 4. Mapuje: DatabaseModel → DomainEntity                     │
│ 5. Vrátí: DomainEntity do Application Layer                 │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ APPLICATION LAYER - Preparing Response                      │
├─────────────────────────────────────────────────────────────┤
│ 1. Mapuje: DomainEntity → ResponseDto (AutoMapper)          │
│ 2. Vrátí: ResponseDto do Controller                         │
└────────────────────┬────────────────────────────────────────┘
                     ↓
┌─────────────────────────────────────────────────────────────┐
│ HTTP RESPONSE (JSON)                                        │
└─────────────────────────────────────────────────────────────┘
```

---

## 🏗️ Architektura - Vrstvy a modely

### Mapování mezi vrstvami

```
┌──────────────────┬──────────────────┬──────────────────────┐
│  APPLICATION     │     DOMAIN       │   INFRASTRUCTURE     │
│  LAYER           │     LAYER        │   LAYER              │
├──────────────────┼──────────────────┼──────────────────────┤
│                  │                  │                      │
│  EntityDto       │  DomainEntity    │  DatabaseModel       │
│  (API Request)   │  (Business Logic)│  (EF Core, DB)       │
│                  │                  │                      │
├──────────────────┼──────────────────┼──────────────────────┤
│ Vlastnosti:      │ Vlastnosti:      │ Vlastnosti:          │
│ - Primitivní     │ - Domain Objects │ - EF Core properties │
│ - Enumerace      │ - Business rules │ - Relationships      │
│ - Kolekce DTO    │ - Validace       │ - JSON properties    │
│ - Null checks    │ - Konstruktory   │ - Navigation props   │
│                  │                  │                      │
└──────────────────┴──────────────────┴──────────────────────┘

                    ↕️ AutoMapper                ↕️ AutoMapper
            (Application Mapper)          (Infrastructure Mapper)

                 DTO ↔ Entity ↔ DbModel
```

---

## 🔄 Mapování mezi modely

### 1. Application → Domain (DTO → Entity)
**Kdy:** Při vytváření/úpravě entity
**Mapper:** `ApplicationMapper` (budoucí)
**Příklad:**
```csharp
// RaceDto (API Request) → Race (Domain Entity)
var race = _mapper.Map<Race>(raceDto);
// ↓
// Přídán: Business logic a validace
```

### 2. Domain → Infrastructure (Entity → DbModel)
**Kdy:** Při ukládání do DB (AddAsync, UpdateAsync)
**Mapper:** `DomainInfrastructureMapper`
**Příklad:**
```csharp
// Race (Domain Entity) → RaceDbModel (DB Model)
RaceDbModel dbModel = _mapper.Map<RaceDbModel>(entity);
// ↓
await dbSet.AddAsync(dbModel);
await db.SaveChangesAsync();
```

### 3. Infrastructure → Domain (DbModel → Entity)
**Kdy:** Při čtení z DB (GetByIdAsync, ListAsync)
**Mapper:** `DomainInfrastructureMapper`
**Příklad:**
```csharp
// RaceDbModel (DB Model) → Race (Domain Entity)
var dbModel = await dbSet.FirstOrDefaultAsync(...);
Race race = _mapper.Map<Race>(dbModel);
```

### 4. Domain → Application (Entity → DTO)
**Kdy:** Při vracení API response
**Mapper:** `ApplicationMapper` (budoucí)
**Příklad:**
```csharp
// Race (Domain Entity) → RaceDto (API Response)
var raceDto = _mapper.Map<RaceDto>(race);
// ↓
// Vráceno jako JSON
```

---

## 📋 Repository - Operace a odpovědnosti

### RaceDbRepository

Implementuje `IRaceRepository` s celým CRUD cyklem a loggingem.

#### Read Operations (Čtení)

| Metoda | Účel | Eager Loading | Vrací |
|--------|------|---------------|-------|
| `GetByIdAsync(id)` | Načíst jednu rasu podle ID | ✅ Treasure + CurrencyGroup | `Race?` |
| `GetByNameAsync(name)` | Načíst rasu podle jména | ✅ Treasure + CurrencyGroup | `Race?` |
| `ListAsync()` | Načíst všechny rasy | ✅ Treasure + CurrencyGroup | `IEnumerable<Race>` |
| `GetPageAsync(page, size)` | Stránkované čtení | ✅ Treasure + CurrencyGroup | `IEnumerable<Race>` |
| `GetByIdsAsync(ids)` | Načíst rasy podle seznamu ID | ✅ Treasure + CurrencyGroup | `IEnumerable<Race>` |
| `GetBySeqencAsync(...)` | Čtení s řazením a stránkováním | ✅ Treasure + CurrencyGroup | `IEnumerable<Race>` |
| `ExistsAsync(id)` | Zkontrolovat existenci | ❌ Žádné | `bool` |

#### Write Operations (Zápis)

| Metoda | Účel | Vrací |
|--------|------|-------|
| `AddAsync(entity)` | Přidat novou rasu | `int` (ID) |
| `UpdateAsync(entity)` | Aktualizovat existující rasu | `int` (ID) |
| `DeleteAsync(id)` | Smazat rasu | `void` |

---

## 🔍 Eager Loading a Entity Navigation

### IncludePattern (Eager Loading)

Všechny read operace používají Include pro načítání related entities:

```csharp
// Exemplo z RaceDbRepository.GetByIdAsync():
var dbModel = await dbSet
    .Include(r => r.Treasure)                    // Include Treasure
        .ThenInclude(t => t.CurrencyGroup)       // Include CurrencyGroup
            .ThenInclude(cg => cg.Currencies)    // Include Currencies
    .FirstOrDefaultAsync(r => r.Id == id);
```

**Proč Include?**
- Bez Include by se Treasure, CurrencyGroup a Currencies neinicializovaly
- Bez eager loading by se vytvářely N+1 SQL queries
- Include zajišťuje jednu SQL query s JOIN

### Related Entities

Následující entity se vždy načítají spolu s Race:

```
Race
├── Treasure (1:1)
│   └── CurrencyGroup (1:1)
│       └── Currencies (1:*)
├── BodyParts (1:*)
├── SpecialAbilities (1:*)
├── RaceHierarchySystem (1:*)
├── Stats (1:*)
├── Vulnerabilities (1:*)
└── Mobility (1:*)
```

---

## 📝 Logging a Performance Tracking

Každá operace je zalogována s:

### Log Information

```csharp
// Příklad logu z GetByIdAsync:
var sw = Stopwatch.StartNew();
var traceId = GetTraceId();  // Unikátní ID pro tracking

try
{
    // Operace...
    sw.Stop();
    
    var logId = LogIdFactory.Create(
        ProjectLayerType.Infrastructure,
        OperationType.Repository,
        LogLevelCodeType.Information,
        EventVariantType.RepositoryRead);
    
    var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
    log.LogResult(logger);  // Log success
}
catch (Exception ex)
{
    // Log error s trackem
    var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
    log.LogResult(logger);
}
```

### Logged Information

- **Operation Type**: Add, Update, Delete, Read
- **Execution Time**: Doba trvání v ms
- **Trace ID**: Unikátní identifikátor pro celý request
- **Status**: OK, Failed, Canceled
- **Exception Details**: Pokud došlo k chybě

---

## 🔐 Cancellation Token Handling

Všechny asynchronní operace podporují `CancellationToken`:

```csharp
public async Task<Race?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
{
    try
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Operace...
    }
    catch (OperationCanceledException)
    {
        // Log timeout
        throw;
    }
}
```

**Výhody:**
- Graceful shutdown
- Timeout handling
- Request cancellation
- Vhodné pro long-running operations

---

## 🛡️ Dependency Injection (DI)

### Registrace v Program.cs (budoucí)

```csharp
// Services registrace
services.AddScoped<IRaceRepository, RaceDbRepository>();
services.AddScoped<ISingleCurrencyRepository, SingleCurrencyDbRepository>();
services.AddScoped<ICurrencyGroupRepository, CurrencyGroupDbRepository>();
services.AddScoped<ITreasureRepository, TreasureDbRepository>();

// AutoMapper profily
services.AddAutoMapper(typeof(DomainInfrastructureMapper));

// DbContext
services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(connection));
```

### Injected Dependencies

Každý repository dostane injected:
- `SqlDbContext` - Pro přístup k DB
- `ILogger` - Pro logging
- `IHttpContextAccessor` - Pro TraceId
- `IMapper` - Pro mapování

---

## ⚙️ Konfigurace a Nastavení

### Stránkování - Výchozí hodnoty

```csharp
// GetPageAsync - Default a limity:
if (page < 1) page = 1;           // Minimum 1
if (size < 1) size = 5;           // Default 5
if (size > 100) size = 100;       // Maximum 100
```

### Řazení - Povolené vlastnosti

```csharp
// GetBySeqencAsync - Povolené sloupce:
var allowedProperties = new[]
{
    nameof(RaceDbModel.Id),
    nameof(RaceDbModel.RaceName),
    nameof(RaceDbModel.RaceCategory),
    nameof(RaceDbModel.BaseInitiative),
    nameof(RaceDbModel.BaseXP),
    nameof(RaceDbModel.FightingSpiritNumber),
    nameof(RaceDbModel.ZSM),
    nameof(RaceDbModel.DomesticationValue)
};
```

**Proč whitelist?**
- SQL Injection prevention
- Performance optimization
- Business logic respect

---

## 🧪 Testování Repository

### Mockování v testech

```csharp
// Test fixture s mock mapper
var mockMapper = new Mock<IMapper>();
mockMapper
    .Setup(m => m.Map<Race>(It.IsAny<RaceDbModel>()))
    .Returns((RaceDbModel dbModel) => dbModel.MapToRace());

var repository = new RaceDbRepository(
    dbContext,
    logger,
    httpContextAccessor,
    mockMapper.Object);
```

### TestDbFixture

```csharp
// In-memory databáze pro unit testy
public class TestDbFixture
{
    public SqlDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new SqlDbContext(options);
        SeedTestData(context);
        return context;
    }
}
```

---

## 🚀 Best Practices

### 1. Vždy používejte Include pro related entities
```csharp
// ✅ SPRÁVNĚ - Include zajistí jednu query
var race = await dbSet
    .Include(r => r.Treasure)
    .FirstOrDefaultAsync();

// ❌ ŠPATNĚ - N+1 query problem
var race = await dbSet.FirstOrDefaultAsync();
var treasure = race.Treasure;  // Další query!
```

### 2. Mapujte na správný typ
```csharp
// ✅ SPRÁVNĚ - Entity → Dto pouze na API vrstvě
public async Task<RaceDto> GetRaceAsync(int id)
{
    var race = await _repository.GetByIdAsync(id);
    return _mapper.Map<RaceDto>(race);
}

// ❌ ŠPATNĚ - Mapování na Dto v repository
return _mapper.Map<RaceDto>(dbModel);
```

### 3. Sempre use CancellationToken
```csharp
// ✅ SPRÁVNĚ
public async Task<Race?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
{
    cancellationToken.ThrowIfCancellationRequested();
    // ...
}

// ❌ ŠPATNĚ - Bez cancellation support
public async Task<Race?> GetByIdAsync(int id)
{
    // ...
}
```

### 4. Logujte operace s tracingem
```csharp
// ✅ SPRÁVNĚ - S stopwatch a trace ID
var sw = Stopwatch.StartNew();
var traceId = GetTraceId();
// ... operace ...
var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);

// ❌ ŠPATNĚ - Bez metrik
// ... operace ...
```

### 5. Validujte vstupní parametry
```csharp
// ✅ SPRÁVNĚ
if (string.IsNullOrEmpty(name))
    throw new ArgumentException("Name cannot be empty", nameof(name));

// ❌ ŠPATNĚ - Bez validace
public async Task<Race?> GetByNameAsync(string name)
{
    // Může selhať s null exception
}
```

---

## 📚 Pokročilé operace

### Řazení a filtrování

```csharp
// GetBySeqencAsync - Dynamic sorting
await _repository.GetBySeqencAsync(
    page: 1,
    size: 10,
    sortBy: nameof(RaceDbModel.RaceName),
    direction: SortDirection.Ascending);
```

### Stránkování

```csharp
// Načíst druhou stránku s 5 položkami
var page2 = await _repository.GetPageAsync(page: 2, size: 5);
// Skip(5).Take(5) - vrátí prvky 6-10
```

### Hromadné operace

```csharp
// Načíst více ras najednou
var raceIds = new[] { 1, 2, 3, 4, 5 };
var races = await _repository.GetByIdsAsync(raceIds);
```

---

## 🔧 Troubleshooting

### Problem: N+1 Query Problem
**Příznaky:** Příliš mnoho SQL queries  
**Řešení:** Přidejte Include() pro related entities

### Problem: DbContext tracking issues
**Příznaky:** Změny se neukládají nebo se opakovaně  
**Řešení:** Ujistěte se, že entita je tracked a SaveChangesAsync() je voláno

### Problem: Slow queries
**Příznaky:** Long response times  
**Řešení:** 
- Přidejte .AsNoTracking() pro read-only queries
- Optimalizujte Include patterns
- Přidejte indexy v databázi

### Problem: Timeout
**Příznaky:** OperationCanceledException  
**Řešení:** Zkontrolujte network a databázi, zvyšte timeout

---

## 📊 Úspěšnost operací

| Operace | Status | Test Coverage |
|---------|--------|----------------|
| GetByIdAsync | ✅ Hotovo | ✅ Otestováno |
| GetByNameAsync | ✅ Hotovo | ✅ Otestováno |
| ListAsync | ✅ Hotovo | ✅ Otestováno |
| GetPageAsync | ✅ Hotovo | ✅ Otestováno |
| GetByIdsAsync | ✅ Hotovo | ✅ Otestováno |
| GetBySeqencAsync | ✅ Hotovo | ✅ Otestováno |
| ExistsAsync | ✅ Hotovo | ✅ Otestováno |
| AddAsync | ✅ Hotovo | ⏳ V procesu |
| UpdateAsync | ✅ Hotovo | ⏳ V procesu |
| DeleteAsync | ✅ Hotovo | ⏳ V procesu |

---

## 📖 Další zdroje

- [Entity Framework Core - Include](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager)
- [Repository Pattern - Microsoft](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
