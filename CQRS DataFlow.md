# 🔧 CQRS DataFlow – Create Command

> Kompletní průchod datovým tokem při vytváření entity pomocí CQRS patternu

---

## 📋 Obsah

- [1. Command](#1-command)
- [2. Validator](#2-validator)
- [3. Handler](#3-handler)
- [4. LoggingBehavior](#4-loggingbehavior-pipeline)
- [Shrnutí](#-shrnutí)

---

## 1️⃣ Command

Definice příkazu pro vytvoření entity:

```csharp
public record CreateRaceCommand(string Name, string Description) : IRequest<Response<Guid>>;
```

**Klíčové vlastnosti:**
- 📦 `record` pro immutabilitu
- 🔗 Implementuje `IRequest<Response<Guid>>`
- 📤 Vrací `Response<Guid>` s ID vytvořené entity

---

## 2️⃣ Validator

Validace vstupních dat pomocí FluentValidation:

```csharp
public class CreateRaceCommandValidator : AbstractValidator<CreateRaceCommand>
{
    public CreateRaceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
```

**Validační pravidla:**
- ✅ `Name` - povinné, max 100 znaků
- ✅ `Description` - nepovinné, max 500 znaků

> 💡 **Tip:** Validator se spouští automaticky přes MediatR pipeline behavior před voláním handleru

---

## 3️⃣ Handler

Handler zpracuje příkaz a vrátí Response s metadaty:

```csharp
public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, Response<Guid>>
{
    private readonly IRaceRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateRaceCommandHandler(
        IRaceRepository repository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<Guid>> Handle(
        CreateRaceCommand request, 
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var traceId = _httpContextAccessor.HttpContext?.Items["TraceId"] as string 
                   ?? Guid.NewGuid().ToString("N");

        // Validace může proběhnout přes pipeline (FluentValidation)
        var id = await _repository.CreateAsync(request.Name, request.Description);
        sw.Stop();

        return Response<Guid>.Ok(id,
            eventId: new EventId((int)EventVariant.EntityCreated, 
                                nameof(EventVariant.EntityCreated)),
            traceId: traceId,
            elapsedMs: sw.ElapsedMilliseconds);
    }
}
```

**Klíčové kroky:**
1. ⏱️ **Měření času** - `Stopwatch.StartNew()`
2. 🔗 **TraceId** - získání z HTTP kontextu pro korelaci
3. 💾 **Vytvoření entity** - volání repository
4. 📊 **Response** - vrácení `Response<Guid>` s metadaty:
   - `Success = true`
   - `Data = Guid` (ID nové entity)
   - `EventId = EntityCreated`
   - `TraceId` pro korelaci
   - `ElapsedMs` pro metriky
   - `StatusCode = 200`

---

## 4️⃣ LoggingBehavior (pipeline)

MediatR pipeline behavior automaticky loguje průběh:

```csharp
// LoggingBehavior zachytí Response<Guid> a zaloguje:
_logger.LogInformation(
    response.EventId ?? LogEvents.CommandHandled,
    "Handled {Command} in {ElapsedMs}ms | TraceId: {TraceId} | Status: {StatusCode}",
    typeof(CreateRaceCommand).Name,
    response.ElapsedMs,
    response.TraceId,
    response.StatusCode
);
```

**Co se loguje:**
- 📝 `EventId = EntityCreated`
- 🔗 `TraceId` - korelační ID požadavku
- ⏱️ `ElapsedMs` - čas zpracování
- 🔢 `StatusCode = 200`
- 📋 Název příkazu

> ⚠️ **Poznámka:** LoggingBehavior se spouští automaticky pro všechny requesty v MediatR pipeline

---

## ✨ Shrnutí

### Tok dat při vytváření entity

```
┌─────────────┐
│   Request   │
│  (Command)  │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Validator  │ ◄─── FluentValidation pipeline
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Handler   │ ◄─── Zpracování logiky
│             │      • Měření času
│             │      • TraceId z context
│             │      • Repository call
│             │      • Response s metadaty
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Logging   │ ◄─── Pipeline behavior
│  Behavior   │      • Automatické logování
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Response   │
│ <Guid>      │
└─────────────┘
```

### Klíčové komponenty

| Komponenta | Odpovědnost | Výstup |
|------------|-------------|--------|
| **Command** | Definuje akci (CreateRace) | `IRequest<Response<Guid>>` |
| **Validator** | Kontroluje vstupy | Validační chyby nebo ✅ |
| **Handler** | Provede akci | `Response<Guid>` s metadaty |
| **LoggingBehavior** | Zachytí průběh a zaloguje | Log entry s TraceId |

### Výhody tohoto přístupu

| Výhoda | Popis |
|--------|-------|
| 🔍 **Trasovatelnost** | TraceId umožňuje sledovat request napříč systémem |
| ⏱️ **Metriky** | ElapsedMs poskytuje data pro monitoring výkonu |
| 📊 **Strukturované logy** | EventId a metadata pro snadné vyhledávání |
| ✅ **Konzistence** | Response pattern napříč všemi operacemi |
| 🧪 **Testovatelnost** | Oddělené komponenty s jasnými odpovědnostmi |

---

## 🎓 Doporučení

1. 🔗 **TraceId middleware**: Vygenerujte TraceId v middleware a ukládejte do `HttpContext.Items`
2. ⏱️ **Metriky**: Použijte `ElapsedMs` pro monitoring a alerting
3. 📝 **EventId**: Centralizujte EventId v `LogEvents` třídě
4. 🧪 **Unit testy**: Testujte validator, handler a response zvlášť
5. 🔍 **Structured logging**: Logujte vždy s EventId a TraceId

---

<div style="text-align: center; color: #888; margin-top: 2em;">
  <small>📝 CQRS DataFlow Pattern | Verze 1.0 | Last Updated: 2025-12-06</small>
</div>
