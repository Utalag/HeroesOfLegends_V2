# CQRS Response Pattern

> Kompletní implementační šablony pro návratové typy v CQRS architektuře

---

## 📋 Obsah

- [Shrnutí](#-shrnutí)
- [Response\<T\> (generický)](#-responset-generický)
- [ResponseBase (non‑generic)](#-responsebase-nongeneric)
- [Mapování na HTTP odpověď](#-mapování-na-http-odpověď)
- [Příklady použití v handlerech](#-příklady-použití-v-handlerech)
- [Integrace s EventId, logováním a metrikami](#-integrace-s-eventid-logováním-a-metrikami)
- [Ukázkový unit test](#-ukázkový-unit-test)
- [Doporučené praktiky](#-doporučené-praktiky)
- [Poznámka k implementaci](#-poznámka-k-implementaci)
- [Konečné shrnutí](#-konečné-shrnutí)

---

## 📋 Shrnutí

**Cíl:** Jednotný kontrakt pro výsledky handlerů, který nese:
- ✅ Stav operace
- 📦 Data (pokud existují)
- ⚠️ Validační chyby
- 🔢 HTTP/aplikační status
- ⏱️ Čas zpracování
- 🔍 EventId pro logování
- 🔗 TraceId pro korelaci

**Výhody:**
- ✔️ Konzistence napříč aplikací
- 🌐 Snadné mapování na HTTP
- 📊 Lepší logování a metriky
- 🧪 Jednodušší testování

---

## 🎯 Response\<T\> (generický)

```csharp
public class Response<T>
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> ValidationErrors { get; init; } = new();
    public T? Data { get; init; }

    // Tracing / logging
    public string? TraceId { get; init; }
    public EventId? EventId { get; init; }

    // Metadata
    public int StatusCode { get; init; }
    public long ElapsedMs { get; init; }

    // Factory metody
    public static Response<T> Ok(T data, EventId? eventId = null, string? traceId = null, long elapsedMs = 0) =>
        new() { Success = true, Data = data, StatusCode = 200, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };

    public static Response<T> Fail(string error, EventId? eventId = null, string? traceId = null, int statusCode = 400, long elapsedMs = 0) =>
        new() { Success = false, ErrorMessage = error, StatusCode = statusCode, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };

    public static Response<T> ValidationFailed(IEnumerable<string> errors, EventId? eventId = null, string? traceId = null, int statusCode = 422, long elapsedMs = 0) =>
        new() { Success = false, ValidationErrors = errors.ToList(), ErrorMessage = "Validation failed", StatusCode = statusCode, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };
}
```

---

## 📌 ResponseBase (non‑generic)

> Pro příkazy bez návratové hodnoty

```csharp
public class ResponseBase
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> ValidationErrors { get; init; } = new();
    public string? TraceId { get; init; }
    public EventId? EventId { get; init; }
    public int StatusCode { get; init; }
    public long ElapsedMs { get; init; }

    public static ResponseBase Ok(EventId? eventId = null, string? traceId = null, long elapsedMs = 0) =>
        new() { Success = true, StatusCode = 200, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };

    public static ResponseBase Fail(string error, EventId? eventId = null, string? traceId = null, int statusCode = 400, long elapsedMs = 0) =>
        new() { Success = false, ErrorMessage = error, StatusCode = statusCode, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };

    public static ResponseBase ValidationFailed(IEnumerable<string> errors, EventId? eventId = null, string? traceId = null, int statusCode = 422, long elapsedMs = 0) =>
        new() { Success = false, ValidationErrors = errors.ToList(), ErrorMessage = "Validation failed", StatusCode = statusCode, TraceId = traceId, ElapsedMs = elapsedMs, EventId = eventId };
}
```

---

## 🌐 Mapování na HTTP odpověď

### Extension metody pro převod na IActionResult

```csharp
public static class ResponseExtensions
{
    public static IActionResult ToActionResult(this ResponseBase r, ControllerBase controller)
    {
        if (r.Success) return controller.StatusCode(r.StatusCode);
        if (r.StatusCode == 404) return controller.NotFound(new { r.ErrorMessage, r.TraceId });
        if (r.StatusCode == 422) return controller.UnprocessableEntity(new { r.ValidationErrors, r.TraceId });
        return controller.StatusCode(r.StatusCode, new { r.ErrorMessage, r.TraceId });
    }

    public static IActionResult ToActionResult<T>(this Response<T> r, ControllerBase controller)
    {
        if (r.Success) return controller.StatusCode(r.StatusCode, r.Data);
        if (r.StatusCode == 404) return controller.NotFound(new { r.ErrorMessage, r.TraceId });
        if (r.StatusCode == 422) return controller.UnprocessableEntity(new { r.ValidationErrors, r.TraceId });
        return controller.StatusCode(r.StatusCode, new { r.ErrorMessage, r.TraceId });
    }
}
```

### Použití v controlleru

```csharp
var result = await _mediator.Send(command);
return result.ToActionResult(this);
```

---

## 💡 Příklady použití v handlerech

### ✅ Command handler vracející ID

```csharp
public async Task<Response<Guid>> Handle(CreateOrderCommand cmd, CancellationToken ct)
{
    var sw = Stopwatch.StartNew();

    var errors = Validate(cmd);
    if (errors.Any())
        return Response<Guid>.ValidationFailed(errors, eventId: LogEvents.ValidationFailed, traceId: GetTraceId(), elapsedMs: sw.ElapsedMilliseconds);

    var id = await _repository.CreateAsync(cmd);
    sw.Stop();

    return Response<Guid>.Ok(id, eventId: LogEvents.EntityCreated, traceId: GetTraceId(), elapsedMs: sw.ElapsedMilliseconds);
}
```

### 🗑️ Command handler bez návratové hodnoty

```csharp
public async Task<ResponseBase> Handle(DeleteOrderCommand cmd, CancellationToken ct)
{
    var sw = Stopwatch.StartNew();

    var errors = Validate(cmd);
    if (errors.Any())
        return ResponseBase.ValidationFailed(errors, eventId: LogEvents.ValidationFailed, traceId: GetTraceId(), elapsedMs: sw.ElapsedMilliseconds);

    var ok = await _repository.DeleteAsync(cmd.Id);
    sw.Stop();

    return ok
        ? ResponseBase.Ok(eventId: LogEvents.EntityDeleted, traceId: GetTraceId(), elapsedMs: sw.ElapsedMilliseconds)
        : ResponseBase.Fail("Delete failed", eventId: LogEvents.UnhandledException, traceId: GetTraceId(), statusCode: 500, elapsedMs: sw.ElapsedMilliseconds);
}
```

---

## 📊 Integrace s EventId, logováním a metrikami

| Doporučení | Popis |
|-----------|-------|
| 📝 **EventId & TraceId** | Logujte při chybách a důležitých událostech |
| 🔗 **TraceId middleware** | Generujte v middleware a propisujte do Response/logů |
| ⏱️ **ElapsedMs měření** | Použijte Stopwatch nebo metrikový middleware |
| 🏗️ **Centralizace** | LogEvents a EventIdFactory v infrastruktuře |

### Příklad logování v handleru

```csharp
_logger.LogInformation(response.EventId ?? LogEvents.EntityCreated, "Handled {Command} in {Elapsed}ms Trace {TraceId}", nameof(CreateOrderCommand), response.ElapsedMs, response.TraceId);
```

## 🧪 Ukázkový unit test

```csharp
[Fact]
public void ValidationFailed_SetsStatusCodeAndErrors()
{
    var errors = new[] { "Name is required" };
    var r = ResponseBase.ValidationFailed(errors, eventId: LogEvents.ValidationFailed, traceId: "trace-1", elapsedMs: 10);

    Assert.False(r.Success);
    Assert.Equal(422, r.StatusCode);
    Assert.Contains("Name is required", r.ValidationErrors);
    Assert.Equal(LogEvents.ValidationFailed, r.EventId);
}
```

---

## ✨ Doporučené praktiky

1. 🔗 **TraceId**: Generujte v middleware a propisujte do Response a logů
2. ⏱️ **ElapsedMs**: Měřte pomocí Stopwatch nebo metrikového middleware
3. 🧪 **CI testy**: Testujte unikátnost EventId a konzistenci StatusCode mapování
4. 🔒 **Bezpečnost**: Logujte strukturovaně, neukládejte citlivé údaje do textových polí

---

## 🔧 Poznámka k implementaci

> ⚠️ Vlož kód do příslušných projektů (infrastruktura, shared kernel) a přizpůsob názvy `LogEvents`, `EventId` a `GetTraceId()` podle existujícího kódu.

---

## 🎓 Konečné shrnutí

> Použitím **Response\<T\>** a **ResponseBase** získáš konzistentní, testovatelný a snadno logovatelný kontrakt pro CQRS handlery, který propojí **API**, **logování** a **metriky** do jedné srozumitelné konvence.

---

<div style="text-align: center; color: #888; margin-top: 2em;">
  <small>📝 Dokument pro CQRS Response Pattern | Verze 1.0</small>
</div>