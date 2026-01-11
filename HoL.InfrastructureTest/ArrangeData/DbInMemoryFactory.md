# DbInMemoryFactory - Best Practices pro Testování

## Shrnutí

`DbInMemoryFactory` je továrna na vytváření In-Memory databází pro unit a integrační testy. Umožňuje:
- ✅ Vytváření nezávislých test databází
- ✅ Sdílení databází mezi kontexty se stejným jménem
- ✅ Jednoduchou inicializaci testů bez SQL serveru

---

## Kdy Použít Co?

### 1. **Každý test potřebuje novou databázi** (nejčastější případ)

```csharp
[Fact]
public void MyTest()
{
    // Každé zavolání vytvoří NOVOU databázi
    var context1 = DbInMemoryFactory.CreateDbContext();
    var context2 = DbInMemoryFactory.CreateDbContext();
    
    // context1 a context2 jsou NEZÁVISLÉ
}
```

**Výhody:**
- Testy jsou nezávislé na sobě
- Žádné pollutování dat mezi testy
- Dobrá izolace

---

### 2. **Plusieurs kontextů potřebují sdílet databázi** (integrační testy)

```csharp
[Fact]
public void MyIntegrationTest()
{
    const string dbName = "MySharedDb";
    
    // Oba kontexty jsou připojeny k STEJNÉ databázi
    var context1 = DbInMemoryFactory.CreateDbContext(dbName);
    var context2 = DbInMemoryFactory.CreateDbContextForExistingDatabase(dbName);
    
    // Data zapsaná v context1 jsou viditelná v context2
    context1.SingleCurrencies.Add(new SingleCurrencyDbModel { Id = 1, Name = "Gold" });
    context1.SaveChanges();
    
    Assert.Single(context2.SingleCurrencies);
}
```

**Výhody:**
- Simulace více klientů pristupujících k stejné databázi
- Testování konkurenčního přístupu

---

### 3. **Použití DbFixture** (doporučeno pro komplexní scénáře)

```csharp
public class MyTest : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    
    public MyTest(DbFixture fixture)
    {
        _fixture = fixture; // Databáze se vytvoří JEDNOU pro všechny testy v třídě
    }
    
    [Fact]
    public void Test1() 
    {
        // Používám _fixture.Context - sdílená databáze
    }
    
    [Fact]
    public void Test2() 
    {
        // Používám _fixture.Context - stejné data jako Test1
    }
}
```

**Výhody:**
- Efektivnější - databáze se vytvoří pouze jednou
- Seed data jsou dostupná všem testům
- Ideální pro repository testy

---

## Srovnění Přístupů

| Přístup | Vytvoření DB | Sdílení Dat | Výkon | Kdy Použít |
|---------|---|---|---|---|
| `CreateDbContext()` (GUID) | Vždy nová | NE | Pomalejší | Unit testy, izolace |
| `CreateDbContext(dbName)` | Nová s jménem | Lze | Střední | Integration testy |
| `DbFixture` | Jednou | ANO | Nejrychlejší | Komplexní scenáře |

---

## Příklady Používání

### Příklad 1: Jednoduchý Unit Test
```csharp
[Fact]
public void Repository_GetById_ReturnsCorrectEntity()
{
    // Arrange
    var context = DbInMemoryFactory.CreateDbContext();
    var repository = new SingleCurrencyRepository(context);
    
    // Act & Assert
    var result = repository.GetById(1);
}
```

### Příklad 2: Test s Více Kontexty
```csharp
[Fact]
public void TwoContexts_ShareDatabase()
{
    const string dbName = "TestDb";
    
    // Arrange
    var context1 = DbInMemoryFactory.CreateDbContext(dbName);
    context1.SingleCurrencies.Add(new SingleCurrencyDbModel { Id = 1, Name = "Gold" });
    context1.SaveChanges();
    context1.Dispose();
    
    // Act
    var context2 = DbInMemoryFactory.CreateDbContextForExistingDatabase(dbName);
    var result = context2.SingleCurrencies.FirstOrDefault(sc => sc.Id == 1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Gold", result.Name);
}
```

### Příklad 3: Fixture Pattern (Doporučeno)
```csharp
public class SingleCurrencyRepositoryTest : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    
    public SingleCurrencyRepositoryTest(DbFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void GetById_WithValidId_ReturnsEntity()
    {
        // Arrange
        var repository = new SingleCurrencyRepository(_fixture.Context);
        
        // Act
        var result = repository.GetById(1); // Gold z seed dat
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gold", result.Name);
    }
}
```

---

## Doporučení

✅ **Doporučuji:**
1. Používat `DbFixture` pro repository/integration testy
2. Používat `DbInMemoryFactory.CreateDbContext()` pro jednoduché unit testy
3. Používat pojmenované databáze pro testování konkurenčního přístupu

❌ **Vyhnout se:**
1. Sdílení databází bez explicitního jména (důvod: chyby jsou těžko diagnostikovatelné)
2. Používání stejné databáze pro všechny testy bez izolace (může dojít k interferenci)

---

## Poznámky k In-Memory Databázím

- **In-Memory databáze jsou VÝCHOZÍ** - data jsou uložena v paměti, ne na disku
- **Když se kontext dispose, databáze zmizí** (pokud je poslední kontext s tímto jménem)
- **Nejsou vhodné pro testování transakcí a lockingů** - použijte SQLite nebo SQL Server pro to
- **GUID jména jsou pro testování ideální** - zajišťují jedinečnost bez konfliktů

---

## Závěr

`DbInMemoryFactory` je jednoduchý a efektivní způsob, jak vytvářet test databáze. Kombinujte ho s `DbFixture` pro nejlepší výsledky v integration testech.
