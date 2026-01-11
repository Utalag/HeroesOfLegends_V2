# Prompt pro vytváření xUnit testovací třídy

## 📋 Účel
Vytvoř komplexní xUnit testovací třídu pro ověřování funkcionality a chování C# třídy podle standardů HoL projektu.

---

## 🏗️ Požadavky na strukturu

### 1. Pojmenovávání třídy a namespace
- **Název testovací třídy**: `{NázevTesovvanéTřídy}Test`
- **Namespace**: `{ProjektNázev}.ObjectValue` pro value objects
- **Namespace**: `{ProjektNázev}.Entities` pro entity objekty
- **Namespace**: `{ProjektNázev}.Helpers` pro utility třídy

**Příklady:**
- `HoL.DomainTest.ObjectValue.SpecialAbilitiesTest`
- `HoL.DomainTest.Entities.RaceTest`
- `HoL.DomainTest.Helpers.DiceTest`

### 2. Hlavička a dokumentace třídy

```csharp
/// <summary>
/// Testovací sada pro třídu <see cref="TestovanáTřída"/>.
/// Ověřuje [seznam hlavních ověřovaných funkcionalit].
/// </summary>
/// <remarks>
/// Testovací scénáře:
/// <list type="number">
/// <item><description>Scenario 1 - Popis</description></item>
/// <item><description>Scenario 2 - Popis</description></item>
/// <item><description>Scenario 3 - Popis</description></item>
/// </list>
/// </remarks>
public class TestovanáTřídaTest
{
```

### 3. Organizace testů do regionů

- Strukturuj testy do `#region` bloků podle testovacích scénářů
- Pojmenovávej regiony: `#region {Název scénáře} - Scenario {Číslo}`
- Každý region obsahuje logicky související testy

```csharp
#region Testovací scénář - Scenario 1

[Fact]
public void Test_...() { ... }

[Fact]
public void Test_...() { ... }

#endregion
```

### 4. AAA Pattern (Arrange-Act-Assert)

Každý test **musí** sledovat striktně AAA pattern:

```csharp
[Fact]
public void Test_ClassName_Behavior()
{
    // Arrange - Příprava testovacích dat a objektů
    const string testData = "value";
    var testObject = new ClassName(testData);
    
    // Act - Vykonání testované funkcionality
    var result = testObject.Method();
    
    // Assert - Ověření výsledků
    Assert.NotNull(result);
    Assert.Equal("expected", result);
}
```

**Rozlišení:**
- `// Arrange` - Inicializace testovacích dat
- `// Act` - Vykonání testované metody
- `// Assert` - Ověření výsledků
- `// Act & Assert` - Pro jednoduché testy bez Arrange

### 5. Dokumentační komentáře pro testy

Každý test musí mít XML dokumentaci s `<summary>`:

```csharp
/// <summary>
/// Ověří, že [konkrétní chování].
/// </summary>
[Fact]
public void Test_...() { ... }
```

**Příklady:**
- "Ověří, že nová instance je vytvořena s správnými hodnotami."
- "Ověří, že metoda změní atribut na novou hodnotu."
- "Ověří, že konstruktor vyhodí výjimku při prázdném názvu."

### 6. Pojmenovávání testovacích metod

```
Test_{NázevTřídy}_{OvěřovanéChování}[_VýjimkaNeměnovat]
```

**Příklady:**
- `Test_SpecialAbilities_Creation()` - základní vytváření
- `Test_SpecialAbilities_WithName()` - změna vlastnosti
- `Test_SpecialAbilities_WithName_NullName_ThrowsException()` - validace s vyjimkou
- `Test_SpecialAbilities_FluentAPI_Chaining()` - chování API
- `Test_Treasure_AddCoins()` - přidání dat
- `Test_Treasure_RemoveCoins_InsufficientFunds_ThrowsException()` - error scenario

### 7. Testování výjimek - ⚠️ KLÍČOVÉ

Pro testy na vyhazování výjimek **MUSÍŠ** dodržet pravidlo:

**✅ SPRÁVNĚ:**
```csharp
[Fact]
public void Test_ClassName_InvalidInput_ThrowsException()
{
    // Act & Assert
    Assert.Throws<ArgumentException>(() => new ClassName(""));
}
```

**❌ ŠPATNĚ:**
```csharp
[Fact]
public void Test_ClassName_InvalidInput_ThrowsException()
{
    // Act & Assert
    var exception = Assert.Throws<ArgumentException>(() => new ClassName(""));
    Assert.Equal("Název nesmí být prázdný", exception.Message); // ❌ NE!
}
```

**Důvody:**
- Testuj **POUZE typ výjimky**, ne obsah zprávy
- Testy jsou odolné vůči změnám textu v chybové zprávě
- Méně břemene údržby kódu

### 8. Fluent API testování

Pokud třída implementuje fluent API (vrací `this`):

**Test vrácení `this`:**
```csharp
/// <summary>
/// Ověří, že metoda vrací `this` pro fluent API.
/// </summary>
[Fact]
public void Test_ClassName_WithProperty_ReturnsSelf()
{
    // Arrange
    var obj = new ClassName("value");

    // Act
    var result = obj.WithProperty("newValue");

    // Assert
    Assert.Same(obj, result); // Ověř, že je to stejný objekt
}
```

**Test řetězení volání:**
```csharp
/// <summary>
/// Ověří, že fluent API umožňuje řetězení více volání metod.
/// </summary>
[Fact]
public void Test_ClassName_FluentAPI_Chaining()
{
    // Arrange & Act
    var obj = new ClassName("value1")
        .WithProperty1("value2")
        .WithProperty2("value3");

    // Assert
    Assert.Equal("value2", obj.Property1);
    Assert.Equal("value3", obj.Property2);
}
```

**Test opakovaných změn:**
```csharp
/// <summary>
/// Ověří, že fluent API umožňuje opakované změny stejného atributu.
/// </summary>
[Fact]
public void Test_ClassName_FluentAPI_MultipleChanges()
{
    // Arrange & Act
    var obj = new ClassName("v1")
        .WithProperty("v2")
        .WithProperty("v3")
        .WithProperty("v4");

    // Assert
    Assert.Equal("v4", obj.Property);
}
```

---

## ✅ Pokrytí testů - Kontrolní seznam

### Základní funkcionality
- ✅ Vytváření instance s platnými hodnotami
- ✅ Vytváření instance s prázdnými/null hodnotami (kde relevantní)
- ✅ Ověření inicializace všech vlastností
- ✅ Ověření výchozích hodnot

### Validace a chyby
- ✅ Vyhazování správných typů výjimek
- ✅ Vyhazování na neplatné vstupy (null, prázdný string, záporná čísla, atd.)
- ✅ Zpracování edge case scénářů
- ✅ Testování hraniční hodnoty (0, -1, max value, atd.)

### Chování metod
- ✅ Správná změna atributů
- ✅ Vrácení `this` u fluent API metod
- ✅ Zpracování null/prázdných hodnot
- ✅ Vedlejší efekty operací
- ✅ Stav objektu po operaci

### Robustnost
- ✅ Speciální znaky v textech (čeština: á, č, ř, ž, atd.)
- ✅ Unicode a lokalizace
- ✅ Dlouhé texty / hraniční délky
- ✅ Hranice datových typů
- ✅ Whitespace handling

### Fluent API (pokud implementován)
- ✅ Řetězení více volání
- ✅ Opakované změny atributů
- ✅ Komplexní kombinace operací
- ✅ Vrácení `this` v každém kroku
- ✅ Order nezávislost (pokud relevantní)

---

## 📐 Struktura regionů podle scénářů

### Pro Value Object (jako SpecialAbilities)
```
#region Vytváření instance - Scenario 1
- Test_ClassName_Creation()
- Test_ClassName_CreationWithEmpty()
#endregion

#region Validace konstruktoru - Scenario 2
- Test_ClassName_Creation_EmptyName_ThrowsException()
- Test_ClassName_Creation_NullName_ThrowsException()
#endregion

#region Metoda WithProperty - Scenario 3
- Test_ClassName_WithProperty()
- Test_ClassName_WithProperty_ReturnsSelf()
- Test_ClassName_WithProperty_InvalidValue_ThrowsException()
#endregion

#region Fluent API řetězení - Scenario 4
- Test_ClassName_FluentAPI_Chaining()
- Test_ClassName_FluentAPI_MultipleChanges()
#endregion

#region Robustnost - Scenario 5
- Test_ClassName_SpecialCharacters()
- Test_ClassName_LongTexts()
#endregion

#region Výchozí hodnoty - Scenario 6
- Test_ClassName_DefaultValues()
#endregion
```

### Pro Entity (jako Race)
```
#region Vytváření instance - Scenario 1
- Test_ClassName_Creation()
- Test_ClassName_Creation_WithDefaults()
#endregion

#region Validace konstruktoru - Scenario 2
- Test_ClassName_Creation_InvalidInput_ThrowsException()
#endregion

#region Metody nastavení - Scenario 3
- Test_ClassName_SetProperty()
- Test_ClassName_SetProperty_InvalidValue_ThrowsException()
#endregion

#region Kolekce - Scenario 4
- Test_ClassName_AddToCollection()
- Test_ClassName_RemoveFromCollection()
#endregion

#region Komplexní operace - Scenario 5
- Test_ClassName_CombinedOperations()
#endregion
```

---

## 🔧 Imports a závislosti

```csharp
using {NázevTestovanéhoNamespace};
using Xunit;

namespace {ProjektNázev}.ObjectValue
{
    public class {ClassName}Test
    {
        // Testy zde...
    }
}
```

**Standardní importy:**
- `using Xunit;` - Pro `[Fact]` atribut a `Assert` třídu
- `using {NázevTestovanéhoNamespace};` - Pro testovanou třídu

---

## 📊 Příklady z HoL projektu

### SpecialAbilitiesTest
- 7 scénářů
- 19 testů
- Pokrývá: vytváření, validaci, fluent API, robustnost
- Regiony pro snadnou navigaci

### TreasureTest
- Pokrývá: vytváření, manipulaci s daty, výpočty
- Fluent API testy
- Edge cases (záporné hodnoty, nedostatek financí)

### DiceTest
- Testuje generování náhodných čísel
- Testuje boundary conditions
- Testuje validaci vstupů

---

## 🎯 Konvence pojmenovávání - Shrnutí

| Typ | Schéma | Příklad |
|-----|--------|---------|
| Třída | `{Třída}Test` | `SpecialAbilitiesTest` |
| Test - základní | `Test_{Třída}_{Akce}` | `Test_SpecialAbilities_Creation` |
| Test - se změnou | `Test_{Třída}_{Metoda}` | `Test_SpecialAbilities_WithName` |
| Test - fluent | `Test_{Třída}_FluentAPI_{Typ}` | `Test_SpecialAbilities_FluentAPI_Chaining` |
| Test - výjimka | `Test_{Třída}_{Akce}_ThrowsException` | `Test_SpecialAbilities_WithName_EmptyName_ThrowsException` |
| Test - robustnost | `Test_{Třída}_{Typ}` | `Test_SpecialAbilities_SpecialCharacters` |

---

## 🛠️ Technické požadavky

- ✅ **.NET** 9.0+
- ✅ **C#** 12.0+
- ✅ **xUnit** v3.0+
- ✅ Všechny testy musí **procházet** (`dotnet test`)
- ✅ Bez **varování** při kompilaci
- ✅ Dodržování **projektových konvencí**
- ✅ **XML dokumentace** pro všechny testy
- ✅ Korektní **AAA pattern**

---

## 🚀 Spuštění testů

```bash
# Spustit všechny testy
dotnet test

# Spustit testy konkrétního projektu
dotnet test HoL.DomainTest

# Spustit testy konkrétní třídy
dotnet test --filter "SpecialAbilitiesTest"

# Verbose output
dotnet test -v normal
```

---

## 📝 Checklist pro vytvoření testovací třídy

- [ ] Vytvořena třídu `{Třída}Test` v `ObjectValue` / `Entities` / `Helpers` adresáři
- [ ] Přidán XML `<summary>` na třídu
- [ ] Přidán XML `<remarks>` s `<list type="number">` scénáři
- [ ] Strukturovány testy do `#region` bloků
- [ ] Všechny testy mají XML `<summary>` komentář
- [ ] Testy používají **AAA pattern** (Arrange-Act-Assert)
- [ ] Testy validace výjimek testují **jen typ**, ne zprávu
- [ ] Fluent API testy testují `Assert.Same()` pro `this`
- [ ] Testovány **edge cases** a **robustnost**
- [ ] Všechny testy **procházejí** (`dotnet test`)
- [ ] Žádná **varování** při kompilaci
- [ ] Dodrženy **konvence pojmenovávání**

---

## 💡 Tipy a triky

### 1. Konstanta vs. proměnná
```csharp
// Preferuj const pro jednoduché hodnoty
const string name = "Test";
const int value = 42;

// Použij var pro objekty
var ability = new SpecialAbilities(name, "description");
```

### 2. Srozumitelné jména testovacích dat
```csharp
// ✅ Dobré
const string validName = "Magická rezistence";
const string emptyDescription = "";

// ❌ Špatné
const string name = "a";
const string desc = "";
```

### 3. Testování null
```csharp
// Null v konstruktoru
Assert.Throws<ArgumentException>(() => new ClassName(null, "value"));

// Null v metodě
Assert.Throws<ArgumentNullException>(() => method.Call(null));
```

### 4. Testování prázdných stringů
```csharp
// Prázdný string
Assert.Throws<ArgumentException>(() => new ClassName("", "value"));

// Whitespace
Assert.Throws<ArgumentException>(() => new ClassName("   ", "value"));
```

---

## 🎯 Soupis co se testuje podle typu dat

Tento přehled vám pomůže vybrat **relevantní testy pro konkrétní typ dat** který testujete.

### 📝 STRING (Textové hodnoty)

**Co se testuje:**
- ✅ Vytváření s platnými hodnotami
- ✅ Vytváření s prázdným stringem (`""`)
- ✅ Vytváření s null
- ✅ Vytváření s samými mezerami (`"   "`)
- ✅ Speciální znaky (čeština: á, č, ř, ž, š, atd.)
- ✅ Unicode znaky (emoji, jiné jazyky)
- ✅ Dlouhé texty (1000+ znaků)
- ✅ Malá/velká písmena
- ✅ Whitespace na začátku/konci (trim vs. bez trimování)
- ✅ Escape sekvence (\n, \t, atd.)
- ✅ HTML/XML znaky (<, >, &, atd.)

**Příklady testů:**
```csharp
// Validní
Test_ClassName_WithName("Magická rezistence")
Test_ClassName_WithName("Test")

// Nevalidní
Test_ClassName_WithName("")                    // Prázdný
Test_ClassName_WithName(null)                  // Null
Test_ClassName_WithName("   ")                 // Samé mezery

// Edge cases
Test_ClassName_SpecialCharacters()             // Čeština: á, č, ř
Test_ClassName_LongTexts()                     // 1000+ znaků
Test_ClassName_WithUnicodeCharacters()         // Emoji, CJK znaky
```

---

### 🔢 ČÍSELNÉ HODNOTY (int, long, decimal, double, float)

**Co se testuje:**
- ✅ Vytváření s kladnými čísly
- ✅ Vytváření s nulou (0)
- ✅ Vytváření se zápornými čísly (pokud jsou povoleny)
- ✅ Minimální hodnota (`int.MinValue`, `0`)
- ✅ Maximální hodnota (`int.MaxValue`)
- ✅ Hraniční hodnoty (0, 1, -1, int.MaxValue, int.MinValue)
- ✅ Přesnost (pro decimal/double: zaokrouhlování)
- ✅ Přetečení (overflow)
- ✅ Dělení nulou (pokud relevantní)

**Příklady testů:**
```csharp
// Validní
Test_ClassName_WithValue(0)                    // Nula
Test_ClassName_WithValue(100)                  // Kladné číslo
Test_ClassName_WithValue(1)                    // Minimální kladné

// Nevalidní (záleží na logice)
Test_ClassName_WithValue(-1)                   // Záporné - vyhazuje výjimku
Test_ClassName_WithValue(int.MinValue)         // Minimální - vyhazuje výjimku

// Edge cases - Hranice
Test_ClassName_WithValue(0)                    // Nula
Test_ClassName_WithValue(1)                    // Počátek
Test_ClassName_WithDecimal_Precision()         // Zaokrouhlování
```

---

### 📅 DATUM A ČAS (DateTime, DateTimeOffset, TimeSpan)

**Co se testuje:**
- ✅ Vytváření s platnými daty
- ✅ Minimální datum (`DateTime.MinValue`)
- ✅ Maximální datum (`DateTime.MaxValue`)
- ✅ Dnešek (`DateTime.Now`, `DateTime.Today`)
- ✅ Hraniční dny (1. ledna, 31. prosince)
- ✅ Přestupné roky (2020, 2024)
- ✅ Časové zóny (UTC vs. lokální)
- ✅ Přesnost (milisekundy)
- ✅ Neplatná data (29. února v nepřestupném roce)

**Příklady testů:**
```csharp
// Validní
Test_ClassName_WithDate(DateTime.Now)
Test_ClassName_WithDate(new DateTime(2024, 1, 1))

// Hraniční
Test_ClassName_WithDate(DateTime.MinValue)
Test_ClassName_WithDate(DateTime.MaxValue)
Test_ClassName_WithDate(new DateTime(2020, 2, 29))  // Přestupný rok

// Operace
Test_ClassName_DatesComparison()                   // Porovnávání
Test_ClassName_DateRangeValidation()               // Rozsah
```

---

### ✅ BOOLEAN (true/false)

**Co se testuje:**
- ✅ Vytváření s `true`
- ✅ Vytváření s `false`
- ✅ Default hodnota (zavísí na typu)
- ✅ Negace (toggle)
- ✅ Logické operace (AND, OR, NOT)

**Příklady testů:**
```csharp
// Jednoduché
Test_ClassName_WithEnabled_True()
Test_ClassName_WithEnabled_False()

// Operace
Test_ClassName_Toggle()
Test_ClassName_LogicalOperations()
```

---

### 📦 KOLEKCE (List<T>, Dictionary<K,V>, IEnumerable<T>)

**Co se testuje:**
- ✅ Vytváření s prázdnou kolekcí
- ✅ Vytváření s jedním prvkem
- ✅ Vytváření s více prvky
- ✅ Přidávání prvků (Add)
- ✅ Odebírání prvků (Remove)
- ✅ Přístup k prvkům (indexer, Keys, Values)
- ✅ Iterace (foreach)
- ✅ Filtrování, mapování (LINQ: Where, Select)
- ✅ Počet prvků (Count)
- ✅ Duplikáty (při relevanci)
- ✅ Null prvky v kolekci
- ✅ Objednanost (pokud relevantní)
- ✅ Kapacita (performance)

**Příklady testů:**
```csharp
// Inicializace
Test_ClassName_Creation_EmptyCollection()
Test_ClassName_Creation_WithItems()

// Operace
Test_ClassName_Add()
Test_ClassName_Remove()
Test_ClassName_AddCoins(coinType, amount)
Test_ClassName_RemoveCoins_InsufficientFunds_ThrowsException()

// Ověření
Test_ClassName_CollectionCount()
Test_ClassName_Iteration()
Test_ClassName_Contains()
```

---

### 🎯 ENUM (Výčtový typ)

**Co se testuje:**
- ✅ Vytváření s každou hodnotou enumu
- ✅ Default hodnota
- ✅ Konverze na string
- ✅ Konverze ze stringu
- ✅ Porovnávání (==)
- ✅ Invalidity hodnota (pokud relevantní)

**Příklady testů:**
```csharp
// Jednotlivé hodnoty
Test_ClassName_WithStatus_Active()
Test_ClassName_WithStatus_Inactive()
Test_ClassName_WithStatus_Pending()

// Operace
Test_ClassName_StatusConversion()
Test_ClassName_StatusComparison()
```

---

### 🏗️ COMPLEX OBJECT (Vlastní třídy, Entity, Value Object)

**Co se testuje:**
- ✅ Vytváření instance
- ✅ Inicializace všech vlastností
- ✅ Výchozí hodnoty
- ✅ Relace s jinými objekty (foreign key, reference)
- ✅ Neplatné kombinace (validace)
- ✅ Mutace (měnění stavu objektu)
- ✅ Nestálost (immutability, pokud relevantní)
- ✅ Porovnávání objektů (equality)
- ✅ Hash code (pokud implementován)
- ✅ Kódování/dekódování (serialization)

**Příklady testů:**
```csharp
// Vytváření
Test_ClassName_Creation()
Test_ClassName_CreationWithDefaults()

// Validace
Test_ClassName_Creation_InvalidProperty_ThrowsException()

// Chování
Test_ClassName_SetProperty()
Test_ClassName_Equality()

// Fluent API
Test_ClassName_FluentAPI_Chaining()
```

---

### 🔑 NULL / OPTIONAL (Nullable<T>, možné null hodnoty)

**Co se testuje:**
- ✅ Vytváření s null
- ✅ Vytváření s hodnotou
- ✅ Konverze na/ze null
- ✅ Kontrola HasValue
- ✅ Metody na null reference (očekávám výjimku)
- ✅ Null-coalescing operátor (`??`)
- ✅ Null-forgiving operátor (`!`)

**Příklady testů:**
```csharp
// Null handling
Test_ClassName_WithNullValue()
Test_ClassName_WithValue()

// Operace
Test_ClassName_NullCoalescing()
Test_ClassName_HasValue()
```

---

### 📊 VZTAHY MEZI DATY

**Co se testuje:**
- ✅ Závislosti mezi vlastnostmi (pokud je změní A, změní se B?)
- ✅ Validace kombinace hodnot (A nemůže být bez B)
- ✅ Hraniční podmínky (A musí být < B)
- ✅ Kauzální vztahy (příčina → následek)
- ✅ Stavy (platné kombinace stavů)

**Příklady testů:**
```csharp
// Vztahy
Test_ClassName_DependentProperties()
Test_ClassName_PropertyCombination()

// Validace
Test_ClassName_CreationWithIncompatibleValues_ThrowsException()
Test_ClassName_AUpdateAffectsB()
```

---

## 📋 Matrica relevantnosti testů podle typu

| Typ | Prázdné/null | Validní | Hraniční | Speciální | Kombinace | Fluent |
|-----|:---:|:---:|:---:|:---:|:---:|:---:|
| **string** | ✅ | ✅ | ⚠️ | ✅ | - | ✅ |
| **int** | ⚠️ | ✅ | ✅ | ⚠️ | - | ⚠️ |
| **DateTime** | ⚠️ | ✅ | ✅ | ✅ | ✅ | ⚠️ |
| **bool** | - | ✅ | - | ⚠️ | ✅ | ⚠️ |
| **Kolekce** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Enum** | ⚠️ | ✅ | - | ⚠️ | ✅ | ⚠️ |
| **Object** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Nullable** | ✅ | ✅ | ⚠️ | ⚠️ | ✅ | ⚠️ |

**Legenda:**
- ✅ = **Vždy testovat** - relevantní a důležité
- ⚠️ = **Někdy testovat** - záleží na kontextu
- \- = **Nevhodné** - není relevantní

---

## 💡 Rozhodovací strom pro výběr testů

```
Ptejte se na sebe:

1. Jaký je TYP hodnoty? (string, int, bool, object, kolekce?)
   └─> Podívej se výše na sekci pro ten typ

2. Je POVINNÁ nebo NEPOVINNÁ?
   ├─> Povinná: Testuj null, prázdné, validní
   └─> Nepovinná: Testuj null a validní

3. Má HRANICE nebo OMEZENÍ?
   ├─> Ano: Testuj hranice (min, max, 0, -1)
   └─> Ne: Testuj normální hodnoty

4. Má SPECIÁLNÍ ZNAKY nebo FORMÁT?
   ├─> Ano: Testuj speciální znaky, unicode, délku
   └─> Ne: Základní string test

5. INTERAGUJE s JINÝMI VLASTNOSTMI?
   ├─> Ano: Testuj kombinace a vztahy
   └─> Ne: Testuj izolovaně

6. Má FLUENT API?
   ├─> Ano: Testuj Assert.Same(), řetězení
   └─> Ne: Přeskoč fluent testy
```

---

## 🎓 Příklady pro různé typy vlastností

### Příklad 1: String vlastnost (Name, Description)
```csharp
#region String Properties - Name

[Fact]
public void Test_Class_WithName_ValidValue()
{
    // Validní
    var obj = new Class("Valid Name");
    Assert.Equal("Valid Name", obj.Name);
}

[Fact]
public void Test_Class_WithName_EmptyString_ThrowsException()
{
    Assert.Throws<ArgumentException>(() => new Class("");
}

[Fact]
public void Test_Class_WithName_Null_ThrowsException()
{
    Assert.Throws<ArgumentException>(() => new Class(null));
}

[Fact]
public void Test_Class_WithName_Whitespace_ThrowsException()
{
    Assert.Throws<ArgumentException>(() => new Class("   "));
}

[Fact]
public void Test_Class_SpecialCharacters()
{
    var obj = new Class("Útok → Obrana [pokročilý]");
    Assert.Equal("Útok → Obrana [pokročilý]", obj.Name);
}

#endregion
```

### Příklad 2: Numerická vlastnost (Quantity, Price)
```csharp
#region Numeric Properties - Quantity

[Fact]
public void Test_Class_WithQuantity_ValidValue()
{
    // Kladné číslo
    var obj = new Class(quantity: 100);
    Assert.Equal(100, obj.Quantity);
}

[Fact]
public void Test_Class_WithQuantity_Zero()
{
    // Nula (test hraniční hodnoty)
    var obj = new Class(quantity: 0);
    Assert.Equal(0, obj.Quantity);
}

[Fact]
public void Test_Class_WithQuantity_NegativeValue_ThrowsException()
{
    // Záporné číslo (pokud nejsou povoleny)
    Assert.Throws<ArgumentOutOfRangeException>(() => 
        new Class(quantity: -1));
}

[Fact]
public void Test_Class_WithQuantity_MaxValue()
{
    // Maximální hodnota (hraniční test)
    var obj = new Class(quantity: int.MaxValue);
    Assert.Equal(int.MaxValue, obj.Quantity);
}

#endregion
```

### Příklad 3: Kolekce vlastnost (Items, Tags)
```csharp
#region Collection Properties - Items

[Fact]
public void Test_Class_Creation_EmptyCollection()
{
    // Prázdná kolekce
    var obj = new Class();
    Assert.Empty(obj.Items);
}

[Fact]
public void Test_Class_AddItem()
{
    // Přidání prvku
    var obj = new Class();
    obj.AddItem("item1");
    Assert.Single(obj.Items);
    Assert.Contains("item1", obj.Items);
}

[Fact]
public void Test_Class_RemoveItem()
{
    // Odebrání prvku
    var obj = new Class();
    obj.AddItem("item1");
    obj.RemoveItem("item1");
    Assert.Empty(obj.Items);
}

[Fact]
public void Test_Class_RemoveNonExistentItem_ThrowsException()
{
    // Pokus o odebrání neexistujícího prvku
    var obj = new Class();
    Assert.Throws<InvalidOperationException>(() => 
        obj.RemoveItem("nonexistent"));
}

#endregion
```

---

## ✨ Finální checklist pro výběr testů

Při psaní testů si kladzopište tyto otázky:

- [ ] Jsem si vědom **typu dat** který testuji?
- [ ] Testuju **validní hodnotu**?
- [ ] Testuju **prázdnou/null** (pokud relevantní)?
- [ ] Testuju **hraniční hodnoty** (0, -1, min, max)?
- [ ] Testuju **speciální znaky** (pokud je to string)?
- [ ] Testuju **neplatné kombinace** (pokud relevantní)?
- [ ] Testuju **vyhazování výjimek** (jen typ, ne zpráva)?
- [ ] Testuju **fluent API** (pokud je implementován)?
- [ ] Jsou testy **srozumitelně pojmenované**?
- [ ] Každý test má **XML dokumentaci**?
- [ ] Všechny testy **procházejí**?
