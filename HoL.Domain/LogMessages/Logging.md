# 📊 Logging Convention - EventId Pattern

> Jednotná konvence pro strukturované logování s EventId v projektu

---

## 📋 Obsah

- [Struktura EventId](#-struktura-eventid)
- [Skládání čísel](#-skládání-čísel)
- [Příklady](#-příklady)
- [EventId Factory](#-eventid-factory)
- [Registr pluginů](#-registr-pluginů)
- [Doporučené praktiky](#-doporučené-praktiky)
- [Šablona LogEvents](#-šablona-logevents)
- [Postup migrace](#-postup-migrace)

---

## 🏗️ Struktura EventId

EventId se skládá z několika částí, každá reprezentovaná enum:

### 1️⃣ ProjectLayer – vrstva projektu "X0000000"

| Kód | Vrstva |
|-----|--------|
| `1` | Domain |
| `2` | Application |
| `3` | Infrastructure |
| `4` | API |
| `9` | Plugin |

### 2️⃣ OperationType – typ operace "0XX00000"

| Kód | Operace |
|-----|------|
| `1` | Query |
| `2` | Command |
| `3` | Repository |
| `4` | Security |
| `5` | External |

### 3️⃣ LogLevelCode – úroveň logu "000X0000"

| Kód | Level |
|-----|-------|
| `0` | Trace |
| `1` | Debug |
| `2` | Information |
| `3` | Warning |
| `4` | Error |
| `5` | Critical |

### 4️⃣ EventVariant "0000XX00"

Konkrétní hláška (např. `EntityCreated`, `EntityUpdated`, `QueryHandled`)

### 5️⃣ PluginId "000000XX"

Unikátní identifikátor pluginu (`01`–`99`), aby se zabránilo kolizím mezi pluginy

---

## 🔢 Skládání čísel

### Základní formula

```
EventId = Layer × 10000 + Operation × 1000 + Level × 100 + Variant
```

### Formula pro pluginy

```
EventId = 9 × 100000 + Operation × 10000 + Level × 1000 + PluginId × 10 + Variant
```

---

## 💡 Příklady

| Popis | Výpočet | EventId |
|-------|---------|---------|
| **Application.Command.Information.EntityCreated** | `2×10000 + 2×1000 + 2×100 + 1` | `22201` |
| **Domain.Query.Information.QueryHandled** | `1×10000 + 1×1000 + 2×100 + 4` | `11204` |
| **PluginId=05.Command.Information.EntityCreated** | `9×100000 + 2×10000 + 2×1000 + 5×10 + 1` | `922501` |

---

## 🏭 EventId Factory

```csharp
public static class EventIdFactory
{
    /// <summary>
    /// Vytvoří EventId pro standardní vrstvy projektu
    /// </summary>
    public static EventId Create(
        ProjectLayer layer, 
        OperationType op, 
        LogLevelCode level, 
        EventVariant variant)
    {
        var id = (int)layer * 10000 
               + (int)op * 1000 
               + (int)level * 100 
               + (int)variant;
        
        return new EventId(id, variant.ToString());
    }

    /// <summary>
    /// Vytvoří EventId specifický pro plugin
    /// </summary>
    public static EventId CreateForPlugin(
        int pluginId, 
        OperationType op, 
        LogLevelCode level, 
        int variant, 
        string name)
    {
        var id = (int)ProjectLayer.Plugin * 100000 
               + (int)op * 10000 
               + (int)level * 1000 
               + pluginId * 10 
               + variant;
        
        return new EventId(id, name);
    }
}
```

---

## 📝 Registr pluginů

> ⚠️ Udržujte soubor `PLUGIN_REGISTRY.md` s tabulkou registrovaných pluginů

### Struktura registru

| PluginId | Plugin Name | Owner Contact | Description | Date Registered |
|----------|-------------|---------------|-------------|-----------------|
| `05` | ExamplePlugin | team@example.com | Zpracování custom entit | 2025-12-04 |
| `06` | AuthPlugin | auth@example.com | Extended authentication | 2025-12-05 |

---

## ✨ Doporučené praktiky

### 1. 🏗️ Centralizace

- Držte **enumy** a **továrnu** v infrastrukturní vrstvě
- Použijte sdílený projekt pro společné konstanty

### 2. 🚀 Statické delegáty

Pro frekventované logy použijte `LoggerMessage.Define`:

```csharp
private static readonly Action<ILogger, string, Exception?> _logEntityCreated =
    LoggerMessage.Define<string>(
        LogLevel.Information,
        LogEvents.EntityCreated,
        "Entity {EntityType} created successfully");

public void LogEntityCreated(string entityType) 
    => _logEntityCreated(_logger, entityType, null);
```

### 3. 🔍 Scopes

Logujte důležité kontextové informace:

```csharp
using (_logger.BeginScope(new Dictionary<string, object>
{
    ["CorrelationId"] = correlationId,
    ["UserId"] = userId,
    ["ElapsedMs"] = elapsedMs
}))
{
    _logger.LogInformation(LogEvents.QueryHandled, "Query processed");
}
```

### 4. ✅ CI kontrola

Implementujte **unit testy** pro:
- ✔️ Unikátnost EventId
- ✔️ Nekolidující PluginId
- ✔️ Správnost formátování

```csharp
[Fact]
public void EventIds_Should_Be_Unique()
{
    var eventIds = typeof(LogEvents)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(EventId))
        .Select(f => ((EventId)f.GetValue(null)!).Id)
        .ToList();

    Assert.Equal(eventIds.Count, eventIds.Distinct().Count());
}
```

### 5. 📚 Dokumentace

- 📝 Průběžně aktualizujte `Logging.md`
- 🔖 Veďte `PLUGIN_REGISTRY.md`
- 📊 Dokumentujte nové EventVariant hodnoty

---

## 🎯 Šablona LogEvents

```csharp
/// <summary>
/// Centralizovaný registr EventId pro strukturované logování
/// </summary>
public static class LogEvents
{
    // ========== QUERIES ==========
    public static readonly EventId QueryHandled = EventIdFactory.Create(
        ProjectLayer.Application, 
        OperationType.Query, 
        LogLevelCode.Information, 
        EventVariant.QueryHandled);

    public static readonly EventId QueryFailed = EventIdFactory.Create(
        ProjectLayer.Application, 
        OperationType.Query, 
        LogLevelCode.Error, 
        EventVariant.QueryFailed);

    // ========== COMMANDS ==========
    public static readonly EventId EntityCreated = EventIdFactory.Create(
        ProjectLayer.Application, 
        OperationType.Command, 
        LogLevelCode.Information, 
        EventVariant.EntityCreated);

    public static readonly EventId EntityUpdated = EventIdFactory.Create(
        ProjectLayer.Application, 
        OperationType.Command, 
        LogLevelCode.Information, 
        EventVariant.EntityUpdated);

    public static readonly EventId EntityDeleted = EventIdFactory.Create(
        ProjectLayer.Application, 
        OperationType.Command, 
        LogLevelCode.Information, 
        EventVariant.EntityDeleted);

    // ========== REPOSITORY ==========
    public static readonly EventId DatabaseQueryExecuted = EventIdFactory.Create(
        ProjectLayer.Infrastructure, 
        OperationType.Repository, 
        LogLevelCode.Debug, 
        EventVariant.DatabaseQueryExecuted);
}
```

---

## 🚀 Postup migrace

### Krok 1: Infrastruktura
```csharp
// Přidejte EventIdFactory a enumy do infrastrukturní knihovny
```

### Krok 2: Centralizace
```csharp
// Vytvořte LogEvents s nejčastějšími EventId
```

### Krok 3: Refaktoring
```csharp
// Postupně nahrazujte ručně formátované texty
_logger.LogInformation("Entity created");  // ❌ Před
_logger.LogInformation(LogEvents.EntityCreated, "Entity {Type} created", entityType);  // ✅ Po
```

### Krok 4: Plugin Registry
```markdown
<!-- Zaveďte PLUGIN_REGISTRY.md a proces přidělování PluginId -->
```

### Krok 5: CI/CD
```csharp
// Přidejte testy pro unikátnost EventId
```

---

## 📊 Výhody tohoto přístupu

| Výhoda | Popis |
|--------|-------|
| 🔍 **Filtrovatelnost** | Snadné vyhledávání v logovacích nástrojích |
| 📈 **Metriky** | Deterministické EventId pro agregaci |
| 🔒 **Bezpečnost** | Eliminace kolizí mezi pluginy |
| 🧪 **Testovatelnost** | Snadná validace v unit testech |
| 📚 **Čitelnost** | Strukturovaný a konzistentní formát |

---

## 🎓 Závěr

> ✨ Tento přístup **zjednodušuje práci** s EventId, **zvyšuje čitelnost** logů a **eliminuje kolize** mezi pluginy. EventId zůstává **deterministické** a snadno **filtrovatelné** v logovacích nástrojích jako Seq, Elasticsearch nebo Application Insights.

---

<div style="text-align: center; color: #888; margin-top: 2em;">
  <small>📝 Logging Convention | Verze 1.0 | Last Updated: 2025-12-06</small>
</div>