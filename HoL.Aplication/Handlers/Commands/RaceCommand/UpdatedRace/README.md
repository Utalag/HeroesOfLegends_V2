# UpdatedRaceHandler - Setup a Použití

## 📦 Instalace NuGet balíčků

```bash
# Ve složce HoL.Aplication
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

# Ve složce API projektu (pokud existuje)
dotnet add package FluentValidation.AspNetCore
```

## 🔧 Registrace v DI kontejneru

### Pro ASP.NET Core API (Program.cs nebo Startup.cs)

```csharp
using FluentValidation;
using HoL.Aplication.Behaviors;
using HoL.Aplication.MyMapper;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrace AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

// 2. Registrace MediatR s pipeline behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(UpdatedRaceHandler).Assembly);
    
    // Přidání ValidationBehavior do pipeline
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// 3. Automatická registrace všech validátorů z assembly
builder.Services.AddValidatorsFromAssemblyContaining<UpdatedRaceCommandValidator>();

// 4. Registrace repositories
builder.Services.AddScoped<IRaceRepository, RaceRepository>(); // implementace v Infrastructure

var app = builder.Build();

// 5. Exception handling middleware pro ValidationException
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = 400,
                Errors = errors
            });
        }
        else if (exception is KeyNotFoundException notFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Resource not found.",
                Status = 404,
                Detail = notFoundException.Message
            });
        }
        else
        {
            // Standardní zpracování ostatních výjimek
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An error occurred while processing your request.",
                Status = 500
            });
        }
    });
});

app.MapControllers();
app.Run();
```

## 🎯 Použití v API Controlleru

```csharp
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HoL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RacesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RacesController> _logger;

        public RacesController(IMediator mediator, ILogger<RacesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Aktualizuje existující rasu.
        /// </summary>
        /// <param name="id">ID rasy k aktualizaci</param>
        /// <param name="raceDto">Aktualizovaná data rasy</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True pokud aktualizace proběhla úspěšně</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateRace(
            int id, 
            [FromBody] RaceDto raceDto, 
            CancellationToken cancellationToken)
        {
            // Kontrola že ID v URL odpovídá ID v body
            if (id != raceDto.RaceId)
            {
                return BadRequest(new 
                { 
                    Title = "ID mismatch", 
                    Detail = $"ID in URL ({id}) does not match ID in body ({raceDto.RaceId})" 
                });
            }

            _logger.LogInformation("Received request to update Race with Id: {RaceId}", id);

            var command = new UpdatedRaceCommand(raceDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new { Success = result, Message = $"Race with Id {id} updated successfully" });
        }
    }
}
```

## 📋 Příklady API requestů

### ✅ Validní request (200 OK)

```http
PUT /api/races/1
Content-Type: application/json

{
  "raceId": 1,
  "raceName": "Elf",
  "raceDescription": "Graceful forest dwellers with long lifespans",
  "raceHistory": "Elves have lived for centuries...",
  "raceCategory": "Humanoid",
  "conviction": "Neutral",
  "baseXP": 50,
  "zsm": 1,
  "domesticationValue": 0,
  "baseInitiative": 3,
  "raceHierarchySystem": ["Elder", "Ranger", "Warrior"],
  "treasure": {
    "gold": 100,
    "silver": 50,
    "copper": 25
  },
  "fightingSpirit": {
    "dangerNumber": 5
  },
  "raceWeapon": {
    "weaponName": "Longbow"
  },
  "bodyDimensins": {
    "raceSize": "Medium",
    "weightMin": 50,
    "weightMax": 80,
    "bodyHeightMin": 160,
    "bodyHeightMax": 190,
    "heightMin": 160,
    "heightMax": 190,
    "maxAge": 750
  },
  "mobility": {
    "Walking": 30
  },
  "specialAbilities": [
    {
      "abilityName": "Darkvision",
      "abilityDescription": "Can see in dim light within 60 feet"
    }
  ],
  "vulnerability": {
    "vulnerabilities": {},
    "resistances": {},
    "immunities": []
  },
  "statsPrimar": {
    "Strength": { "min": 6, "diceCount": 2, "diceType": "D6", "max": 16 },
    "Agility": { "min": 8, "diceCount": 2, "diceType": "D6", "max": 18 }
  }
}
```

**Response:**
```json
{
  "success": true,
  "message": "Race with Id 1 updated successfully"
}
```

### ❌ Nevalidní request - chybějící povinná pole (400 Bad Request)

```http
PUT /api/races/1
Content-Type: application/json

{
  "raceId": 0,
  "raceName": "",
  "baseXP": -100
}
```

**Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "RaceDto.RaceId": [
      "RaceId must be greater than 0 for update operation"
    ],
    "RaceDto.RaceName": [
      "RaceName is required"
    ],
    "RaceDto.BaseXP": [
      "BaseXP must be non-negative"
    ]
  }
}
```

### ❌ Entity not found (404 Not Found)

```http
PUT /api/races/99999
```

**Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Resource not found.",
  "status": 404,
  "detail": "Race with Id 99999 not found."
}
```

## 🧪 Unit Testing

### Test validátoru

```csharp
using FluentValidation.TestHelper;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using Xunit;

public class UpdatedRaceCommandValidatorTests
{
    private readonly UpdatedRaceCommandValidator _validator;

    public UpdatedRaceCommandValidatorTests()
    {
        _validator = new UpdatedRaceCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RaceDto_Is_Null()
    {
        var command = new UpdatedRaceCommand(null!);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RaceDto);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_RaceId_Is_Invalid(int invalidId)
    {
        var command = new UpdatedRaceCommand(new RaceDto { RaceId = invalidId });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("RaceDto.RaceId");
    }

    [Fact]
    public void Should_Have_Error_When_RaceName_Is_Empty()
    {
        var command = new UpdatedRaceCommand(new RaceDto 
        { 
            RaceId = 1,
            RaceName = string.Empty 
        });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("RaceDto.RaceName");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var command = new UpdatedRaceCommand(new RaceDto 
        { 
            RaceId = 1, 
            RaceName = "Elf",
            BaseXP = 50,
            // ... minimální validní data
        });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
```

### Test handleru

```csharp
using AutoMapper;
using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using HoL.Aplication.Interfaces;
using HoL.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class UpdatedRaceHandlerTests
{
    private readonly Mock<IRaceRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UpdatedRaceHandler>> _loggerMock;
    private readonly UpdatedRaceHandler _handler;

    public UpdatedRaceHandlerTests()
    {
        _repositoryMock = new Mock<IRaceRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UpdatedRaceHandler>>();
        _handler = new UpdatedRaceHandler(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Race_Successfully()
    {
        // Arrange
        var raceDto = new RaceDto { RaceId = 1, RaceName = "Elf" };
        var command = new UpdatedRaceCommand(raceDto);
        var existingRace = new Race { RaceId = 1, RaceName = "OldElf" };
        var updatedRace = new Race { RaceId = 1, RaceName = "Elf" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRace);
        _mapperMock.Setup(m => m.Map<Race>(raceDto)).Returns(updatedRace);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Race>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedRace, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Race_Not_Exists()
    {
        // Arrange
        var raceDto = new RaceDto { RaceId = 999, RaceName = "NonExistent" };
        var command = new UpdatedRaceCommand(raceDto);

        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Race?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Race>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
```

## ✅ Checklist pro deployment

- [ ] NuGet balíčky nainstalovány (`FluentValidation`, `FluentValidation.DependencyInjectionExtensions`)
- [ ] ValidationBehavior zaregistrován v MediatR pipeline
- [ ] Validátory zaregistrovány v DI (`AddValidatorsFromAssembly...`)
- [ ] AutoMapper zaregistrován
- [ ] Repository zaregistrována v DI
- [ ] Exception handling middleware nakonfigurován
- [ ] Unit testy napsány a procházejí
- [ ] Integration testy s real DB (volitelné)
- [ ] Swagger dokumentace aktualizována (pokud používáte)

## 📝 Poznámky

- **ValidationBehavior běží PŘED handlerem** - handler dostane vždy validní data
- **FluentValidation je asynchronní** - podporuje async validaci (např. DB checks)
- **Validační chyby jsou strukturované** - snadné zobrazení na frontendu
- **Logování** - ValidationBehavior loguje všechny validační chyby
- **Performance** - validátory běží paralelně pro lepší výkon
