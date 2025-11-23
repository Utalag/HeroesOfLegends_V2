MyProjectSolution/
│
├── MyProject.Domain/            	
│   ├── Entities/                	(Domain entities)
│   ├── ValueObjects/            	(Value objects)
│   ├── Enums/                   	(Enums)
│   ├── Interfaces/              	(Interfaces defining domain services)
│   └── MyProject.Domain.csproj
│
├── MyProject.Application/       	
│   ├── Commands/                	(CQRS commands)
│   ├── Queries/                 	(CQRS queries)
│   ├── Handlers/                	(MediatR handlers for commands & queries)
│   ├── DTOs/                    	(Data transfer objects)
│   ├── Interfaces/              	(Application interfaces like repositories)
│   ├── Behaviours/              	(Pipeline behaviours — logging, validation, transactions)
│   └── MyProject.Application.csproj
│
├── MyProject.Infrastructure/    	
│   ├── Data/                    	(DbContext, EF configurations)
│   ├── Repositories/            	(Repository implementations for IRaceRepository...)
│   ├── Services/                	(External integrations)
│   └── MyProject.Infrastructure.csproj
│
├── MyProject.Presentation/      	
│   ├── Controllers/             	(Controllers that inject IMediator and send Commands/Queries)
│   ├── Models/                  	(API models / request/response if different from DTOs)
│   └── MyProject.Presentation.csproj
│
├── MyProject.Tests/             	(Unit/integration tests)
│   ├── ApplicationTests/
│   ├── DomainTests/
│   └── MyProject.Tests.csproj
│
└── MyProject.sln


CQRS + MediatR - ASCII diagram provázání vrstev

Presentation (API) zasílá příkazy/queries přes MediatR => Application Handlers => Infrastructure / Domain

       +--------------------+                           +--------------------+
       |  Presentation/API  |                           |   Client / UI      |
       |--------------------|                           |--------------------|
       | Controllers        |                           | - volá endpointy   |
       | (inject IMediator) |                           +--------------------+
       +---------+----------+
                 |
                 | POST / GET (HTTP)
                 v
       +---------+------------------+
       |     MediatR (IMediator)    |  <--- centrální bod pro zasílání Command/Query/Notification
       +---------+------------------+
                 |
     +-----------+-----------+---------------------------+
     |                       |                           |
     v                       v                           v
+----+------+         +------+-----+              +------+------+
| Command   |         | Query       |              | Notification|
| Handlers  |         | Handlers    |              | Handlers    |
+----+------+         +------+-----+              +------+------+
     |                       |                           |
     v                       v                           v
+----+-------+        +------+-----+               +------+------+
| Application|        | Application |              | Application |
| Services / |        | Read models |              | Behaviours  |
| Use-cases  |        | (DTOs)      |              | (logging,   |
+----+-------+        +------+------+              | validation) |
     |                       |                     +-------------+
     v                       v
+----+-----------------------------------------------+
| Infrastructure (Repositories, DbContext, External) |
| - repositories implement application interfaces    |
| - persistence / transactions                       |
+----------------------------------------------------+
     |
     v
+----+-------+      <--- Domain entities, ValueObjects, Enums
| Domain     |
| - entities |
| - VO       |
+------------+

Poznámky a doporučení
- Controllers nemají volat repository přímo; posílají Command/Query do MediatR.
- Handlery v Application vrstvě mapují DTO -> Domain entity a volají repository (injected IRaceRepository z Infrastructure).
- Repositáře vrací nebo nastavují domain entity (např. Race s RaceId nastaveným po persistenci).
- Pipeline behaviours (Application) slouží k cross-cutting concern: logging, validation (FluentValidation), transakční scope.
- Pro návratové hodnoty Commandů volte zřetelný kontrakt: ID (int), DTO nebo specifický ValueObject (např. created name). Pokud potřebujete více informací, vraťte DTO.
- Pro veřejné API oddělte API modely od domain modelů a používejte mapování (AutoMapper nebo ruční mapy).

Typický tok vytvoření rasy (CreateRace):
1) Client -> POST /api/races (body = CreateRaceDto)
2) Controller přijme request a zavolá _mediator.Send(new CreatedRaceCommand(createDto))
3) MediatR najde CreatedRaceHandler : IRequestHandler<CreatedRaceCommand, T>
4) Handler validuje/mapuje -> zavolá _repository.AddAsync(domainRace)
5) Infrastructure persistuje (DbContext.SaveChanges) a nastaví RaceId
6) Handler vrátí ID nebo RaceDto
7) Controller vrátí HTTP 201 Created s výstupem