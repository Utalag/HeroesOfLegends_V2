# XML Komentáře - Tahák pro C# dokumentaci

Kompletní guide pro psaní XML dokumentačních komentářů v C# (.NET 9).

## 🎯 Základní struktura

```csharp
/// <summary>
/// Stručný popis co třída/metoda/vlastnost dělá (1-2 věty).
/// </summary>
/// <remarks>
/// Detailnější vysvětlení, poznámky, příklady použití.
/// Může obsahovat více odstavců.
/// </remarks>
public class MyClass
{
}
```

## 📋 Kompletní seznam XML tagů

### 1. `<summary>` - Základní popis (POVINNÝ)
```csharp
/// <summary>
/// Validator pro CurrencyDto - validace měnových hodnot.
/// </summary>
public class CurrencyDtoValidator { }
```

### 2. `<remarks>` - Detailní poznámky
```csharp
/// <summary>
/// Vytvoří novou rasu v databázi.
/// </summary>
/// <remarks>
/// Tato metoda automaticky nastaví RaceId po uložení.
/// Používá transakční scope pro zajištění konzistence dat.
/// </remarks>
public async Task<Race> CreateRaceAsync(Race race) { }
```

### 3. `<param>` - Popis parametru
```csharp
/// <summary>
/// Aktualizuje existující rasu.
/// </summary>
/// <param name="raceId">ID rasy k aktualizaci (musí být > 0)</param>
/// <param name="raceDto">Aktualizovaná data rasy</param>
/// <param name="cancellationToken">Token pro zrušení operace</param>
public async Task UpdateRaceAsync(int raceId, RaceDto raceDto, CancellationToken cancellationToken) { }
```

### 4. `<returns>` - Návratová hodnota
```csharp
/// <summary>
/// Získá rasu podle ID.
/// </summary>
/// <param name="id">ID rasy</param>
/// <returns>RaceDto pokud nalezeno, jinak null</returns>
public async Task<RaceDto?> GetRaceByIdAsync(int id) { }

/// <summary>
/// Validuje Currency DTO.
/// </summary>
/// <returns>True pokud validace proběhla úspěšně, jinak false s chybami</returns>
public bool Validate(CurrencyDto dto) { }
```

### 5. `<exception>` - Dokumentace výjimek
```csharp
/// <summary>
/// Vytvoří novou rasu.
/// </summary>
/// <param name="race">Race entita k vytvoření</param>
/// <exception cref="ArgumentNullException">Pokud race je null</exception>
/// <exception cref="ValidationException">Pokud validace selže</exception>
/// <exception cref="DbUpdateException">Pokud dojde k chybě při ukládání do DB</exception>
public async Task CreateAsync(Race race) { }
```

### 6. `<example>` - Příklady použití
```csharp
/// <summary>
/// Validuje měnové hodnoty.
/// </summary>
/// <example>
/// Příklad použití:
/// <code>
/// var validator = new CurrencyDtoValidator();
/// var currency = new CurrencyDto { Gold = 100, Silver = 50 };
/// var result = validator.Validate(currency);
/// if (!result.IsValid)
/// {
///     foreach (var error in result.Errors)
///     {
///         Console.WriteLine(error.ErrorMessage);
///     }
/// }
/// </code>
/// </example>
public class CurrencyDtoValidator { }
```

### 7. `<code>` - Ukázka kódu
```csharp
/// <summary>
/// Převede Race entitu na DTO.
/// </summary>
/// <example>
/// <code>
/// var race = new Race { RaceName = "Elf", BaseXP = 50 };
/// var dto = _mapper.Map&lt;RaceDto&gt;(race);
/// </code>
/// </example>
```

**Poznámka:** V XML komentářích použij `&lt;` místo `<` a `&gt;` místo `>`.

### 8. `<see>` - Odkaz na jiný typ/metodu
```csharp
/// <summary>
/// Handler pro vytvoření rasy.
/// Používá <see cref="IRaceRepository"/> pro persistenci.
/// </summary>
/// <seealso cref="UpdatedRaceHandler"/>
/// <seealso cref="RaceDtoValidator"/>
public class CreatedRaceHandler { }
```

### 9. `<seealso>` - Související typy
```csharp
/// <summary>
/// DTO pro rasu.
/// </summary>
/// <seealso cref="Race"/>
/// <seealso cref="RaceDtoValidator"/>
/// <seealso cref="CreatedRaceCommand"/>
public class RaceDto { }
```

### 10. `<value>` - Popis vlastnosti
```csharp
/// <summary>
/// Název rasy.
/// </summary>
/// <value>
/// Název může obsahovat pouze písmena, čísla, mezery a pomlčky.
/// Maximální délka je 100 znaků.
/// </value>
public string RaceName { get; set; }
```

### 11. `<typeparam>` - Generický parametr
```csharp
/// <summary>
/// Handler pro zpracování MediatR requestů.
/// </summary>
/// <typeparam name="TRequest">Typ requestu implementující IRequest</typeparam>
/// <typeparam name="TResponse">Typ odpovědi</typeparam>
public class Handler<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> { }
```

### 12. `<para>` - Nový odstavec v remarks
```csharp
/// <summary>
/// Komplexní validátor.
/// </summary>
/// <remarks>
/// <para>
/// První odstavec s popisem základní funkcionality.
/// </para>
/// <para>
/// Druhý odstavec s dodatečnými informacemi.
/// </para>
/// </remarks>
```

### 13. `<list>` - Seznamy
```csharp
/// <summary>
/// Validator validuje následující pravidla:
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>
/// <description>RaceName nesmí být prázdný</description>
/// </item>
/// <item>
/// <description>BaseXP musí být 0-100000</description>
/// </item>
/// <item>
/// <description>ZSM musí být 0-20</description>
/// </item>
/// </list>
/// </remarks>
```

**Typy listů:**
- `type="bullet"` - odrážky
- `type="number"` - číslované
- `type="table"` - tabulka

### 14. `<c>` - Inline kód
```csharp
/// <summary>
/// Nastaví <c>RaceId</c> automaticky po uložení.
/// Používá <c>await _repository.AddAsync()</c> pro persistenci.
/// </summary>
```

### 15. `<inheritdoc>` - Zdědit dokumentaci
```csharp
public interface IRepository<T>
{
    /// <summary>
    /// Získá entitu podle ID.
    /// </summary>
    Task<T?> GetByIdAsync(int id);
}

public class RaceRepository : IRepository<Race>
{
    /// <inheritdoc />
    public async Task<Race?> GetByIdAsync(int id) { }
}
```

## 🎨 Praktické příklady

### Příklad 1: DTO třída
```csharp
/// <summary>
/// Data Transfer Object pro reprezentaci měnových hodnot v aplikaci.
/// </summary>
/// <remarks>
/// <para>
/// Currency podporuje tři typy měn: Gold (zlato), Silver (stříbro) a Copper (měď).
/// Všechny hodnoty jsou nullable - pokud není měna zadána, považuje se za 0.
/// </para>
/// <para>
/// Používá se v:
/// <list type="bullet">
/// <item><description>RaceDto.Treasure</description></item>
/// <item><description>CharacterDto.Inventory</description></item>
/// <item><description>ShopDto.Price</description></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// Příklad vytvoření currency:
/// <code>
/// var treasure = new CurrencyDto 
/// { 
///     Gold = 100, 
///     Silver = 50, 
///     Copper = 25 
/// };
/// </code>
/// </example>
/// <seealso cref="CurrencyDtoValidator"/>
public class CurrencyDto
{
    /// <summary>
    /// Počet zlatých mincí.
    /// </summary>
    /// <value>
    /// Hodnota musí být >= 0 pokud je zadána.
    /// Null znamená že zlaté mince nejsou součástí této měny.
    /// </value>
    public int? Gold { get; set; }

    /// <summary>
    /// Počet stříbrných mincí.
    /// </summary>
    /// <value>
    /// Hodnota musí být >= 0 pokud je zadána.
    /// Null znamená že stříbrné mince nejsou součástí této měny.
    /// </value>
    public int? Silver { get; set; }

    /// <summary>
    /// Počet měděných mincí.
    /// </summary>
    /// <value>
    /// Hodnota musí být >= 0 pokud je zadána.
    /// Null znamená že měděné mince nejsou součástí této měny.
    /// </value>
    public int? Copper { get; set; }
}
```

### Příklad 2: Validator třída
```csharp
/// <summary>
/// FluentValidation validator pro CurrencyDto.
/// Zajišťuje že všechny měnové hodnoty jsou validní.
/// </summary>
/// <remarks>
/// <para>
/// Validator kontroluje:
/// <list type="bullet">
/// <item><description>Všechny hodnoty musí být >= 0</description></item>
/// <item><description>Alespoň jedna měna musí být zadána</description></item>
/// </list>
/// </para>
/// <para>
/// Tento validator je znovupoužitelný v jakémkoli DTO obsahujícím CurrencyDto.
/// </para>
/// </remarks>
/// <seealso cref="CurrencyDto"/>
/// <seealso cref="RaceDtoValidator"/>
public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
{
    /// <summary>
    /// Inicializuje novou instanci <see cref="CurrencyDtoValidator"/>.
    /// Definuje všechna validační pravidla.
    /// </summary>
    public CurrencyDtoValidator()
    {
        // Validační pravidla...
    }
}
```

### Příklad 3: Handler třída
```csharp
/// <summary>
/// MediatR handler pro zpracování příkazu aktualizace rasy.
/// </summary>
/// <remarks>
/// <para>
/// Handler provádí následující kroky:
/// <list type="number">
/// <item><description>Validace vstupu (automaticky přes ValidationBehavior)</description></item>
/// <item><description>Kontrola existence entity v databázi</description></item>
/// <item><description>Mapování DTO na domain entitu</description></item>
/// <item><description>Aktualizace přes repository</description></item>
/// <item><description>Logování výsledku</description></item>
/// </list>
/// </para>
/// <para>
/// Handler automaticky loguje všechny operace včetně chyb.
/// Při selhání vyhodí odpovídající výjimku pro middleware.
/// </para>
/// </remarks>
/// <example>
/// Použití v controlleru:
/// <code>
/// [HttpPut("{id}")]
/// public async Task&lt;IActionResult&gt; Update(int id, RaceDto dto)
/// {
///     var command = new UpdatedRaceCommand(dto);
///     var result = await _mediator.Send(command);
///     return Ok(result);
/// }
/// </code>
/// </example>
/// <seealso cref="UpdatedRaceCommand"/>
/// <seealso cref="RaceDtoValidator"/>
/// <seealso cref="IRaceRepository"/>
public class UpdatedRaceHandler : IRequestHandler<UpdatedRaceCommand, bool>
{
    private readonly IRaceRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatedRaceHandler> _logger;

    /// <summary>
    /// Inicializuje novou instanci <see cref="UpdatedRaceHandler"/>.
    /// </summary>
    /// <param name="repository">Repository pro přístup k Race entitám</param>
    /// <param name="mapper">AutoMapper instance pro mapování DTO</param>
    /// <param name="logger">Logger pro zaznamenávání operací</param>
    /// <exception cref="ArgumentNullException">
    /// Pokud některý z parametrů je null
    /// </exception>
    public UpdatedRaceHandler(
        IRaceRepository repository, 
        IMapper mapper,
        ILogger<UpdatedRaceHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Zpracuje příkaz aktualizace rasy.
    /// </summary>
    /// <param name="request">Command s daty pro aktualizaci</param>
    /// <param name="cancellationToken">Token pro zrušení operace</param>
    /// <returns>
    /// True pokud aktualizace proběhla úspěšně, jinak vyhodí výjimku.
    /// </returns>
    /// <exception cref="ArgumentNullException">Pokud request je null</exception>
    /// <exception cref="KeyNotFoundException">Pokud rasa s daným ID neexistuje</exception>
    /// <exception cref="DbUpdateException">Pokud dojde k chybě při ukládání</exception>
    public async Task<bool> Handle(UpdatedRaceCommand request, CancellationToken cancellationToken)
    {
        // Implementace...
    }
}
```

### Příklad 4: Enum
```csharp
/// <summary>
/// Kategorie velikosti rasy/entity.
/// </summary>
/// <remarks>
/// Používá se pro určení fyzické velikosti entit v herním světě.
/// Každá kategorie má pevně daný rozsah výšky.
/// </remarks>
public enum RaceSize
{
    /// <summary>
    /// Miniaturní velikost - do 0,5m (například víly, pixies).
    /// </summary>
    A0 = 1,

    /// <summary>
    /// Velmi malá velikost - 0,5m až 1,5m (například goblini, trpaslíci).
    /// </summary>
    A = 2,

    /// <summary>
    /// Malá až střední velikost - 1,5m až 2m (například lidé, elfové).
    /// </summary>
    B = 3,

    /// <summary>
    /// Střední až velká velikost - 2m až 3m (například orkové, trolové).
    /// </summary>
    C = 4,

    /// <summary>
    /// Velká velikost - 3m až 5m (například obři, ogři).
    /// </summary>
    D = 5,

    /// <summary>
    /// Velmi velká velikost - 5m až 10m (například draci, obří trolové).
    /// </summary>
    E = 6,

    /// <summary>
    /// Gigantická velikost - nad 10m (například titáni, staří draci).
    /// </summary>
    F = 7
}
```

## 🔧 Best Practices

### ✅ DO:
```csharp
/// <summary>
/// Validuje že Gold je >= 0.
/// </summary>
/// <param name="gold">Počet zlatých mincí k validaci</param>
/// <returns>True pokud validní</returns>
public bool ValidateGold(int gold)
```

### ❌ DON'T:
```csharp
/// <summary>
/// Metoda pro validaci
/// </summary>
public bool ValidateGold(int gold) // Vágní, neříká CO validuje
```

### ✅ DO:
```csharp
/// <summary>
/// Získá rasu podle ID z databáze.
/// </summary>
/// <param name="id">ID rasy (musí být > 0)</param>
/// <returns>Race entita nebo null pokud nenalezeno</returns>
/// <exception cref="ArgumentException">Pokud id je <= 0</exception>
```

### ❌ DON'T:
```csharp
/// <summary>
/// Get race
/// </summary>
public async Task<Race?> GetRace(int id) // Příliš stručné
```

## 📝 Šablony pro rychlé použití

### Šablona pro DTO
```csharp
/// <summary>
/// DTO pro [NÁZEV ENTITY] - [STRUČNÝ POPIS].
/// </summary>
/// <remarks>
/// [DETAILNÍ POPIS, POUŽITÍ, POZNÁMKY]
/// </remarks>
/// <seealso cref="[RELATED_CLASS]"/>
public class [NAME]Dto
{
    /// <summary>
    /// [POPIS VLASTNOSTI].
    /// </summary>
    /// <value>
    /// [PRAVIDLA, ROZSAH, FORMÁT]
    /// </value>
    public [TYPE] [PROPERTY] { get; set; }
}
```

### Šablona pro Validator
```csharp
/// <summary>
/// FluentValidation validator pro <see cref="[DTO_NAME]"/>.
/// [STRUČNÝ POPIS CO VALIDUJE].
/// </summary>
/// <remarks>
/// Validator kontroluje:
/// <list type="bullet">
/// <item><description>[PRAVIDLO 1]</description></item>
/// <item><description>[PRAVIDLO 2]</description></item>
/// </list>
/// </remarks>
/// <seealso cref="[DTO_NAME]"/>
public class [NAME]Validator : AbstractValidator<[DTO_NAME]>
{
    /// <summary>
    /// Inicializuje novou instanci <see cref="[NAME]Validator"/>.
    /// </summary>
    public [NAME]Validator() { }
}
```

### Šablona pro Handler
```csharp
/// <summary>
/// MediatR handler pro [POPIS OPERACE].
/// </summary>
/// <remarks>
/// Handler provádí:
/// <list type="number">
/// <item><description>[KROK 1]</description></item>
/// <item><description>[KROK 2]</description></item>
/// </list>
/// </remarks>
/// <seealso cref="[COMMAND_NAME]"/>
public class [NAME]Handler : IRequestHandler<[COMMAND], [RESPONSE]>
{
    /// <summary>
    /// Zpracuje [COMMAND_NAME].
    /// </summary>
    /// <param name="request">[POPIS]</param>
    /// <param name="cancellationToken">Token pro zrušení</param>
    /// <returns>[POPIS NÁVRATOVÉ HODNOTY]</returns>
    /// <exception cref="[EXCEPTION_TYPE]">[PODMÍNKA]</exception>
    public async Task<[RESPONSE]> Handle([COMMAND] request, CancellationToken cancellationToken)
    {
    }
}
```

## 🚀 Generování dokumentace

### DocFX (doporučeno pro .NET)
```bash
# Instalace DocFX
dotnet tool install -g docfx

# Inicializace
docfx init

# Generování HTML dokumentace
docfx build
```

### Visual Studio
- Tools → Options → Text Editor → C# → Advanced
- ✅ Generate XML documentation file

### .csproj
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn> <!-- Potlačí warning pro chybějící XML komentáře -->
</PropertyGroup>
```

## 📖 Reference

- [Microsoft XML Documentation Comments](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/)
- [DocFX](https://dotnet.github.io/docfx/)
- [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB)
