# TODO - HeroesOfLegends CQRS Projekt

## ✅ Dokončené úkoly

### 1. Databázové modely
- ✅ Vytvořit databázové modely (RaceDbModel, CurrencyGroupDbModel, SingleCurrencyDbModel, TreasureDbModel, BodyDimensionDbModel)
- ✅ Konfigurace EF Core pro všechny modely
- ✅ Vytvoření migrací

### 2. Repositáře
- ✅ Vytvořit **GenericRepository** - base třída pro všechny repository
- ✅ Vytvořit **RaceDbRepository** - specializovaný repository pro Race s loggingem
- ✅ Vytvořit **CurrencyGroupDbRepository** - repository pro CurrencyGroup
- ✅ Vytvořit **SingleCurrencyDbRepository** - repository pro SingleCurrency
- ✅ Vytvořit **TreasureDbRepository** - repository pro Treasure (value object)

### 3. Mapování (DbModel ↔ Domain)
- ✅ Vytvořit mapovací třídy bez dědění z GenericRepository:
  - ✅ **RaceDbModelToRaceConverterExtensions.cs** - Converter + extension metoda s RaceBuilder
  - ✅ **SingleCurrencyConverterExtensions.cs** - Converter + extension metoda
  - ✅ **CurrencyGroupConverterExtensions.cs** - Converter + extension metoda
  - ✅ **TreasureConverterExtensions.cs** - Converter + extension metoda (s JSON deserializací)
- ✅ Refaktor: Oddělení converterů a extensions do samostatných souborů
- ✅ **DomainInfrastructureMapper** - AutoMapper profil (Domain → Database mapping)
  - ✅ Integrace converterů do AutoMapper konfiguraci
  - ✅ Mapování Race s plnou logikou (JSON deserializace, RaceBuilder)
  - ✅ Mapování Treasure s načtením CurrencyGroup z DbContext

### 4. Builder Pattern
- ✅ Opravy **RaceBuilderTest.cs** - Všechny testy procházejí
  - ✅ Opraveny chyby ve volání `AddStat()` - používá Dictionary místo jednotlivých argumentů
  - ✅ Opraveny chyby ve volání `AddRaceHierarchy()` - používá List<string> místo jednotlivých stringů
- ✅ Přidána metoda **Load(Race)** - Načítá existující rasu do builderu pro editaci

### 5. RaceDbModelToRaceConverterExtensions - Opravy
- ✅ Opraveny chyby v mapování:
  - ✅ `AddStat()` nyní přijímá celý Dictionary místo jednotlivých prvků
  - ✅ `AddVulnerabilities()` nyní přijímá Dictionary místo WithVulnerability
  - ✅ `AddMobility()` nyní přijímá Dictionary místo jednotlivých prvků
  - ✅ `AddRaceHierarchy()` nyní přijímá List<string> místo jednotlivých stringů

### 6. ArrangeData - Synchronizované testovací data
- ✅ **ArrangeClass.cs** - Kompletní testovací data
  - ✅ Všechny Arrange metody vrací správné typy (singleton místo listů kde je to vhodné)
  - ✅ **Race Constants** - Definovány všechny konstanty pro Elf a Red Dragon
  - ✅ **RaceDbModel_Arrange()** - Vrací list RaceDbModel s mapováním z constnt
  - ✅ **Race_Arrange()** - Vrací list Race s RaceBuilder a identickými daty
  - ✅ **JSON Serialization Helpers** - Kompletní serializace všech kolekcí

### 7. Testy Mapování (20 testů ✅)
- ✅ **RaceDbModelToRaceMappingTest.cs** (10 testů)
  - ✅ Mapování základních vlastností
  - ✅ Mapování tělesných rozměrů
  - ✅ Mapování JSON serializovaných statistik
  - ✅ Mapování zranitelností
  - ✅ Mapování mobility
  - ✅ Mapování hierarchie
  - ✅ Mapování speciálních schopností
  - ✅ Mapování Red Dragon
  - ✅ Správa českých znaků v JSON
  - ✅ Konzistence dat

- ✅ **CurrencyGroupMappingTest.cs** (4 testy)
  - ✅ Mapování základních vlastností
  - ✅ Mapování jednotlivých měn
  - ✅ Zachování pořadí měn
  - ✅ Konzistence mapování

- ✅ **TreasureMappingTest.cs** (6 testů)
  - ✅ Mapování základních vlastností
  - ✅ Mapování měnové skupiny
  - ✅ Deserializace JSON s mincemi
  - ✅ Zachování ID měnové skupiny
  - ✅ Konzistence mapování
  - ✅ Konzistence mincí

### 8. Testy Repository - SingleCurrency (16 testů ✅)
- ✅ **SingleCurrencyRepoTest.cs** (16 testů)
  - ✅ Read Operations (11 testů)
    - ✅ GetByIdAsync - vrátí správnou entitu, null pro neexistující
    - ✅ GetByNameAsync - vrátí měnu podle jména, null pro neexistující
    - ✅ ListAsync - vrátí všechny měny
    - ✅ ExistsAsync - true/false podle existence
    - ✅ GetByIdsAsync - vrátí měny pro seznam ID
    - ✅ GetPageAsync - stránkování (první a druhá stránka)
  - ✅ Write Operations (5 testů)
    - ✅ AddAsync - přidá novou měnu
    - ✅ UpdateAsync - aktualizuje měnu, vyhodí výjimku pro neexistující
    - ✅ DeleteAsync - odstraní měnu, bezpečně zvládne neexistující ID

### 9. TestDbFixture
- ✅ **TestDbFixture.cs** - Změněn z internal na public
- ✅ In-memory databáze se seedem z ArrangeData
- ✅ Initialization všech testovacích dat (SingleCurrencies, CurrencyGroups, Races)

### 10. DTO vrstva (Application)
- ✅ **DTOs existují** - Všechny DTOs jsou vytvořeny pro domain objekty:
  - ✅ EntitiDtos: `RaceDto.cs`
  - ✅ ValueObjectDtos: `CurrencyDto.cs`, `SpecialAbilitiesDto.cs`, `TreasureDto.cs`, `WeaponDto.cs`, `FightingSpiritDto.cs`
  - ✅ AnatomiDtos: `AnatomyProfileDto.cs`, `BodyPartDto.cs`, `BodyPartAttackDto.cs`, `BodyPartDefenseDto.cs`, `DiceDto.cs`
  - ✅ StatDtos: `StatDto.cs`, `ValueRangeDto.cs`
- ✅ **Validátoři** - Pro každý DTO existuje validátor (FluentValidation)
- ✅ **DTOs README.md** - Dokumentace struktur DTOs a mapování

### 11. RaceBuilder rozšíření
- ✅ Přidána metoda `Load(Race race)` - Pro načítání existující rasy do builderu

---

## 🔄 V Průběhu / Částečně hotovo

### Mapování Treasure
- ⚠️ **MapToRace** - Řešeno, ale s omezením:
  - ✅ MapTreasure teď pracuje s DbContext
  - ⚠️ Treasure se mapuje pouze pokud je DbContext dostupný (nullable)
  - ⚠️ Synchronní volání (FirstOrDefault) v logice - ideálně by mělo být async

### Mapování Domain ↔ DTO
- ⚠️ **AutoMapper profil pro DTO mapování** - Neúplné:
  - ✅ DTOs existují
  - ⚠️ Mapování Domain → DTO (pro API responses) - v procesu
  - ⚠️ Mapování DTO → Domain (pro API requests) - v procesu
  - ⚠️ FluentValidation v DI - v procesu

---

## 📋 Zbývající úkoly

### 1. Registrace DI (Dependency Injection) - 🔴 KRITICKÉ
- [ ] Vytvořit extension metodu pro registraci všech repositářů v Program.cs
- [ ] Zaregistrovat `DomainInfrastructureMapper` s SqlDbContext
- [ ] Zaregistrovat všechny repository v DI kontejneru:
  - [ ] `IRaceRepository` → `RaceDbRepository`
  - [ ] `CurrencyGroupDbRepository`
  - [ ] `SingleCurrencyDbRepository`
  - [ ] `TreasureDbRepository`
- [ ] Zajistit, aby RaceDbModelToRaceConverter měl přístup k SqlDbContext
- [ ] Zaregistrovat všechny FluentValidation validátory

### 2. AutoMapper profil pro DTO mapování - 🔴 KRITICKÉ
- [ ] Vytvořit **ApplicationMapper** profil (Domain → DTO mapování)
  - [ ] Race → RaceDto
  - [ ] CurrencyGroup → CurrencyGroupDto (pokud existuje)
  - [ ] SingleCurrency → CurrencyDto
  - [ ] Treasure → TreasureDto
  - [ ] SpecialAbilities → SpecialAbilitiesDto
  - [ ] BodyPart → BodyPartDto
  - [ ] Všechny Anatomie DTO
  - [ ] Všechny Stat DTO
- [ ] Zaregistrovat ApplicationMapper v DI
- [ ] Zajistit snadné mapování v Application vrstvě (Extension metody)

### 3. Testy Repository - Zbývající entity - 🟡 DŮLEŽITÉ
- [ ] **RaceRepoTest** - Testy pro RaceDbRepository
  - [ ] GetByIdAsync - vrátí rasu s mapem na Race doménu
  - [ ] GetByNameAsync - vrátí rasu podle jména
  - [ ] ListAsync - vrátí všechny rasy
  - [ ] AddAsync, UpdateAsync, DeleteAsync
  - [ ] Ověření správného mapování Treasure v rase
  
- [ ] **CurrencyGroupRepoTest** - Testy pro CurrencyGroupDbRepository
  - [ ] GetByIdAsync, GetByNameAsync, ListAsync
  - [ ] Ověření mapování kolekce SingleCurrency
  - [ ] Add, Update, Delete operace

- [ ] **TreasureRepoTest** - Testy pro TreasureDbRepository
  - [ ] GetByIdAsync - vrátí poklad s korektním mapem Treasure
  - [ ] Deserializace JSON mincí
  - [ ] Ověření CurrencyGroup mapování

### 4. DTO mapování testy - 🟡 DŮLEŽITÉ
- [ ] **DomainToDtoMappingTest.cs** - Testy mapování Domain → DTO
  - [ ] Race → RaceDto
  - [ ] SingleCurrency → CurrencyDto
  - [ ] SpecialAbilities → SpecialAbilitiesDto
  - [ ] BodyPart → BodyPartDto
  - [ ] Treasure → TreasureDto
  - [ ] Zachování všech vlastností
  - [ ] Správa null hodnot

- [ ] **DtoToDomainMappingTest.cs** - Testy mapování DTO → Domain
  - [ ] Vytvoření domain objektů z DTOs
  - [ ] Validace vstupních DTO

### 5. Refaktor MapToRace (async Treasure mapování) - 🟡 DŮLEŽITÉ
- [ ] Přepsat `MapToRace` na async verzi (pokud to RaceBuilder umožňuje)
- [ ] Nahradit synchronní `FirstOrDefault` asynchronním `FirstOrDefaultAsync`
- [ ] Zajistit, aby RaceDbRepository.GetByIdAsync správně mapoval Treasure

### 6. Optimalizace TestDbFixture - 🟢 NICE-TO-HAVE
- [ ] Generovat unikátní jména in-memory databází pro paralelní testy
- [ ] Zajistit izolaci dat mezi jednotlivými testy
- [ ] Přidat možnost seedování vlastních dat

### 7. Integration testy - 🟢 NICE-TO-HAVE
- [ ] Vytvořit integration testy pro end-to-end mapování
- [ ] Testy pro komplexní scénáře (Race s Treasure, CurrencyGroup se všemi měnami)
- [ ] Testy pro deserializaci složitých JSON struktur
- [ ] Testy pro DTO validaci a mapování

### 8. API vrstva (budoucí) - ⏳ PLÁNOVÁNO
- [ ] Vytvořit HoL.API projekt
- [ ] Vytvořit Controllers pro Race, Currency, atd.
- [ ] Zaregistrovat MediatR handlery
- [ ] Vytvořit endpoint mapování (Application → API responses)

### 9. CQRS implementace - ⏳ PLÁNOVÁNO
- [ ] Vytvořit Commands pro vytváření/úpravu entit
- [ ] Vytvořit Queries pro čtení dat
- [ ] Registrovat MediatR handlery
- [ ] Testy CQRS logiky

### 10. Dokumentace - ⏳ PLÁNOVÁNO
- [ ] Aktualizovat README s architekturou
- [ ] Dokumentovat MapToRace logiku a proč je async problém
- [ ] Dokumentovat DI registraci v Program.cs
- [ ] Vytvořit dokumentaci testovací strategie
- [ ] Dokumentovat DTO mapování workflow

---

## 🐛 Známé problémy

### 1. MapToRace synchronní volání DbContext
**Problém:** V `RaceDbModelToRaceConverterExtensions.MapTreasure` se volá synchronní `FirstOrDefault`
```csharp
var currencyGroupDbModel = dbContext.Set<CurrencyGroupDbModel>()
    .FirstOrDefault(cg => cg.Id == dbModel.CurrencyId);  // ❌ Synchronní!
```
**Dopad:** Může blokovat thread v async kontextu  
**Řešení:** Refaktor na async (vyžaduje RaceBuilder cambovatelný na async)  
**Priorita:** 🟡 Střední (funguje, ale není optimální)

### 2. DbContext dependency v RaceDbModelToRaceConverter
**Problém:** Converter musí mít přístup k SqlDbContext, ale AutoMapper nevytváří instance s DI
**Řešení:** DomainInfrastructureMapper dostane SqlDbContext v konstruktoru a předá jej converteru  
**Priorita:** 🔴 Kritické (bez DI se nepoužívá)

### 3. Treasure mapování v RaceDbRepository
**Problém:** RaceDbRepository nyní používá `_mapper.Map<Race>(dbModel)`, ale to nemusí správně mapovat Treasure
**Řešení:** Ověřit pomocí testů (test CreateRepository mockuje IMapper)  
**Priorita:** 🔴 Kritické (musí se otestovat)

### 4. JSON deserializace pro SpecialAbilities
**Problém:** System.Text.Json nemůže deserializovat SpecialAbilities z JSON kvůli neshodě mezi konstruktorem a JSON vlastnostmi
**Řešení:** Přidat atributy `[JsonConstructor]` a `[JsonPropertyName]` na třídu SpecialAbilities
**Priorita:** 🔴 Kritické (blokuje mapování speciálních schopností)
**Status:** ✅ Zjištěno, čeká na oprava

---

## 📊 Architektura - Mapování toku

```
HTTP Request (JSON)
    ↓
┌─────────────────────────────────┐
│ API Controller                  │
│ inject IApplicationService      │
└────────────┬────────────────────┘
             │ _service.GetRaceAsync(id)
             v
┌─────────────────────────────────┐
│ Application Service             │
│ inject IRaceRepository          │
│ inject IMapper (DTO)            │
└────────────┬────────────────────┘
             │ await _repo.GetByIdAsync(id)
             v
┌─────────────────────────────────┐
│ RaceDbRepository                │
│ inject IMapper (Domain)         │
│ await dbSet.FindAsync(id)       │
└────────────┬────────────────────┘
             │ _mapper.Map<Race>(dbModel)
             v
┌──────────────────────────────────────────────┐
│ AutoMapper (DomainInfrastructureMapper)       │
│ RaceDbModel → Race (via Converter)           │
└────────────┬───────────────────────────────────┘
             │
             ├─→ RaceDbModelToRaceConverter.Convert()
             │   ↓
             │   MapToRace(dbModel, dbContext)
             │   ├─→ RaceBuilder fluent API
             │   ├─→ Deserializace JSON (BodyParts, Stats)
             │   └─→ MapTreasure(dbModel, dbContext)
             │       ↓
             │       Load CurrencyGroup from DB
             │       ↓
             │       TreasureDbModel → Treasure
             │
             └─→ Return Race with all properties + Treasure
                 ↓
┌──────────────────────────────────────────────┐
│ AutoMapper (ApplicationMapper)                │
│ Race → RaceDto (pro API response)            │
└────────────┬───────────────────────────────────┘
             │
             └─→ Return RaceDto (JSON)
```

---

## 📈 Postup projektu

### Hotové (✅)
- [x] Databázový vrstva (EF Core, modely, konfigurace)
- [x] Repositáře (všechny repozitáře)
- [x] Mapování DbModel ↔ Domain (všechny convertory)
- [x] Builder Pattern (RaceBuilder se Load metodou)
- [x] ArrangeData (synchronizovaná testovací data)
- [x] Testy Mapování (20 testů)
- [x] Testy SingleCurrency Repository (16 testů)
- [x] DTO vrstva (struktury a validátoři)

### V Procesu (🔄)
- [ ] AutoMapper mapování Domain ↔ DTO (střední priorita)
- [ ] Registrace DI kontejneru (kritické!)
- [ ] Zbývající Repository Testy (Race, CurrencyGroup, Treasure)

### Plánováno (⏳)
- [ ] API vrstva (Controllers, DTO mapování)
- [ ] CQRS handlery
- [ ] Integration testy
- [ ] Dokumentace

---

## 🚀 Priorita úkolů

### Vysoká priorita (🔴 Kritické - Bloating)
1. [x] ✅ Opravy RaceBuilder a konverterů
2. [x] ✅ Vytvoření ArrangeData s synchronizovanými konstantami
3. [x] ✅ Testy mapování
4. [x] ✅ Testy SingleCurrency Repository
5. [ ] **Zaregistrovat DI kontejner** - bez toho se nic nepoužívá
6. [ ] **Vytvořit ApplicationMapper** - Domain → DTO mapování
7. [ ] **Opravit JSON deserializaci SpecialAbilities** - blokuje mapování

### Střední priorita (🟡 Důležité - Musí se udělat)
8. [ ] Zbývající Repository Testy (Race, CurrencyGroup, Treasure)
9. [ ] DTO mapování testy
10. [ ] Refaktor MapToRace na async
11. [ ] Integration testy

### Nízká priorita (🟢 Nice-to-have - Když zbyde čas)
12. [ ] Optimalizace TestDbFixture
13. [ ] API vrstva
14. [ ] CQRS implementace
15. [ ] Benchmarking mapovacího procesu