# Architektura - Skeleton (Clean Architecture)

Stru�n� skeleton cel� architektury pro �e�en� HeroesOfLegends. Obsahuje navr�en� projekty, jejich odpov�dnosti, p��klady soubor� a jednoduch� sch�ma prov�z�n�.

---

## Doporu�en� projekty
- HeroesOfLegends.Domain
  - Obsah: entitn� t��dy (Race, Character,...), ValueObjects, Enums, dom�nov� v�jimky a invariants.
  - P��klad soubor�: Entities/Race.cs, ValueObjects/AnatomyProfile.cs, Enums.cs

- HeroesOfLegends.Application
  - Obsah: rozhran� repozit��� (IRaceRepository), DTO (RaceDto, CreateRaceDto), use-casy / handlery (GetRaceQuery, CreateRaceCommand), mapovac� profily, valida�n� pravidla.
  - P��klad soubor�: Interfaces/IRaceRepository.cs, DTO/RaceDto.cs, UseCases/CreateRace/CreateRaceHandler.cs

- HeroesOfLegends.Infrastructure (nebo HeroesOfLegends.DataLayer jako infrastruktura)
  - Obsah: EF DbContext, konfigurace entit, implementace repozit��� (RaceRepository : IRaceRepository), migrace, seed.
  - P��klad soubor�: Persistence/WorldDbContext.cs, Repositories/RaceRepository.cs

- HeroesOfLegends.API (Presentation)
  - Obsah: Web API kontrolery, konfigurace DI, mapov�n� DTO <-> API modely, autorizace, endpointy.
  - P��klad soubor�: Controllers/RaceController.cs, Program.cs (registrace DI)

- HeroesOfLegends.Tests (unit/integration)
  - Obsah: testy pro Domain a Application use-casy.

---

## Typick� struktura adres��� (v projektu)

HeroesOfLegends.Application/
- Interfaces/
  - IRaceRepository.cs
- DTO/
  - RaceDto.cs
  - CreateRaceDto.cs
- UseCases/
  - CreateRace/
    - CreateRaceCommand.cs
    - CreateRaceHandler.cs
- Mapping/
  - AutoMapperProfile.cs

HeroesOfLegends.Domain/
- Entities/
  - Race.cs
- ValueObjects/
  - AnatomyProfile.cs
- Enums.cs

HeroesOfLegends.Infrastructure/
- Persistence/
  - WorldDbContext.cs
- Repositories/
  - RaceRepository.cs
- Migrations/

HeroesOfLegends.API/
- Controllers/
  - RaceController.cs
- Program.cs

---

## Uk�zkov� kontrakty (p�ehled, neimplementace)

- IRaceRepository (v Application.Interfaces)
  - Task<Race?> GetByIdAsync(int id);
  - Task<IEnumerable<Race>> ListAsync();
  - Task AddAsync(Race race);
  - Task UpdateAsync(Race race);
  - Task DeleteAsync(int id);

- RaceDto (v Application.DTO)
  - int Id
  - string Name
  - string Description
  - Dictionary<string, string> Stats // zjednodu�en� pro DTO

- CreateRaceCommand / Handler (v Application.UseCases)
  - Validace vstupu -> p�evod na Domain entity -> vol�n� IRaceRepository.AddAsync

- RaceRepository (v Infrastructure)
  - Implementuje IRaceRepository, pou��v� WorldDbContext

---

## P��kazy pro rychl� vytvo�en� projekt� (CLI)

- dotnet new classlib -n HeroesOfLegends.Domain
- dotnet new classlib -n HeroesOfLegends.Application
- dotnet new classlib -n HeroesOfLegends.Infrastructure
- dotnet new webapi -n HeroesOfLegends.API
- dotnet sln add HeroesOfLegends.Domain/HeroesOfLegends.Domain.csproj
- dotnet sln add HeroesOfLegends.Application/HeroesOfLegends.Application.csproj
- dotnet sln add HeroesOfLegends.Infrastructure/HeroesOfLegends.Infrastructure.csproj
- dotnet sln add HeroesOfLegends.API/HeroesOfLegends.API.csproj

---

## DI registrace (schematicky, v Program.cs API projektu)

- Infrastructure zaregistruje implementace rozhran� definovan�ch v Application (p��klad):
  - services.AddScoped<IRaceRepository, RaceRepository>();
  - services.AddDbContext<WorldDbContext>(options => ...);
- Application registruje slu�by/use-casy a AutoMapper profily.
- API z�vis� na Application rozhran�ch a vol� use-casy.

---

## Pravidla z�vislost�
- API -> Application -> Domain
- Infrastructure -> Application/Domain (implementace rozhran�)
- Domain nez�vis� na Application ani Infrastructure

---

## ASCII sch�ma prov�z�n� vrstev

  +----------------+       uses       +------------------+       uses       +-------------------+
  |   HeroesOf-    |  <-----------   |  HeroesOfLegends  |  <-----------   |  HeroesOfLegends   |
  |   Legends.API  |  ---calls--->   |  .Application     |  ---depends---> |  .Domain (core)    |
  +----------------+                 +------------------+                 +-------------------+
          ^                                                                       ^
          |                                                                       |
          |                                                                       |
  DI registers                                                              implemented by
  services (WorldDbContext,                                                       |
  repositories)                                                                   |
          |                                                                       |
          v                                                                       |
  +------------------+      implements    +------------------------------+
  | HeroesOfLegends  |  <--------------   | HeroesOfLegends.Infrastructure |
  |   (Presentation) |                   |  (DataLayer / EF / Repositories) |
  +------------------+                   +------------------------------+

Legenda:
- �ipky "calls" a "uses" znamenaj� vol�n�/metody
- Infrastructure obsahuje konkr�tn� implementace rozhran� z Application
- Domain je nez�visl� j�dro s entitami a pravidly

---

## Doporu�en� kroky migrace ze sou�asn�ho DataLayer
1. Vytvo�te projekty Domain a Application.
2. P�esu�te entity (Race, Enums, ValueObjects) do Domain.
3. V Application definujte rozhran� repozit���, DTO a use-casy.
4. V Infrastructure (st�vaj�c� DataLayer) implementujte rozhran� z Application.
5. V API zaregistrujte DI a odstra�te p��m� z�vislosti na Infrastructure v kontrolerech (pou��vejte use-casy).

---

Pokud chcete, mohu nyn� vygenerovat skeleton projekt� a vytvo�it nastartovac� soubory (IRaceRepository, RaceDto, CreateRaceCommand, RaceRepository, WorldDbContext, Program.cs) v repozit��i. Napi�te "vytvo� skeleton" pro automatickou generaci.