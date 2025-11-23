# Clean Architecture - Doporuèení pro rozvržení projektù

Krátké a praktické doporuèení pro rozvržení øešení podle principù Clean Architecture. Cílem je jasné oddìlení domény, orchestrace use-caseù a implementace infrastruktury.

## Doporuèené projekty
- HeroesOfLegends.Domain (nebo BusinessLayer)
  - Entities, ValueObjects, Domainové výjimky, doménové služby, invariants (pravidla, která pøímo mìní stav).
  - Tato vrstva by nemìla záviset na žádné jiné vrstvì.

- HeroesOfLegends.Application
  - Use case (Commands / Queries), DTO (Request/Response), rozhraní repozitáøù (napø. IRaceRepository), IUnitOfWork, mapovací profily, validaèní pravidla per use-case.
  - Orchestrace operací a ovìøování vstupù (appli­cation-level validation).
  - Závisí na Domain (a pøípadnì na Common knihovnách), nikoliv na Infrastructure.

- HeroesOfLegends.Infrastructure (nebo ponechat stávající DataLayer jako Infrastructure)
  - Implementace repozitáøù (napø. EF Core DbContext, migrace), implementace IUnitOfWork, pøipojení k DB, seed data.
  - Závisí na Application (pro implementaci rozhraní) a Domain (entitách).

- HeroesOfLegends.API (Presentation)
  - Web API / UI, kontrolery, mapování DTO ? entita, kompozice DI.
  - Závisí na Application (volá use-casy), ne na Infrastructure.

## Kde dát repozitáøe, DTO a validaci
- Rozhraní repozitáøù (IRepository, IRaceRepository) umístit do Application (èi Domain, pokud preferujete domain-driven pøístup). Application definuje kontrakty, Infrastructure je implementuje.
- DTO a mapování: Application (prezentace pracuje jen s DTO; mapování provede Application nebo Presentation podle potøeby).
- Validace vstupù (forma/semantika) a orchestrace: Application. Doménová invarianta a pravidla, která zmìnu stavu zamítají, v Domain.

## Závislosti a smìr závislostí
- Presentation -> Application -> Domain
- Infrastructure implementuje rozhraní definovaná v Application/Domain (zatímco závisí na nich)
- Domain nesmí záviset na Application ani Infrastructure
- Cílem: implementace (Infrastructure) mùže být vymìnìna bez zmìny Application/Domain

## Praktický postup migrace existujícího DataLayer
1. Vytvoøte projekty Application a Domain.
2. Pøesuòte entitní tøídy a value objects (Race, Enums, ValueObjects) do Domain.
3. V Application definujte rozhraní repozitáøù a DTO.
4. V DataLayer/Infrastructure implementujte rozhraní z Application (napø. RaceRepository : IRaceRepository).
5. V Presentation (API) injektujte IRepo z Application; DI registrace provede Infrastructure.

## Rychlé pøíkazy (CLI)
- dotnet new classlib -n HeroesOfLegends.Domain
- dotnet new classlib -n HeroesOfLegends.Application
- dotnet sln add HeroesOfLegends.Domain/HeroesOfLegends.Domain.csproj
- dotnet sln add HeroesOfLegends.Application/HeroesOfLegends.Application.csproj

## Doporuèení a best practices
- DTO a mapování (AutoMapper) držet v Application; Presentation pracuje jen s DTO.
- Používat rozhraní pro repozitáøe, aby Infrastructure byla zamìnitelná.
- Validace: vstupní validaèní pravidla (FluentValidation) v Application; invarianta entity v Domain.
- Unit testy: testovat Application (use-casy) a Domain (invariants) izolovanì.

---

Pokud chcete, pøipravím skeleton projektù (csproj + základní rozhraní IRaceRepository, IUnitOfWork, pøíklad DTO a DI registraci). Napište "vytvoø skeleton" a já to vygeneruji pøímo do øešení.