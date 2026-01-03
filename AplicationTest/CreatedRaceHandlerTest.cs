using AutoMapper;
using HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Contracts;
using HoL.Domain.Entities;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AplicationTest;

public class CreatedRaceHandlerTest
{
    private readonly Mock<IRaceRepository> _repository = new();
    private readonly Mock<ILogger<CreatedRaceHandler>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();


    private CreatedRaceHandler CreateHandler()
    {
        // Nastavení HttpContextu s TraceIdentifier
        var httpContextMock = new Moq.Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.TraceIdentifier).Returns("test-trace-id");
        _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContextMock.Object);


        return new CreatedRaceHandler(
            _repository.Object,
            _logger.Object,
            _mapper.Object,
            _httpContextAccessor.Object);
    }

    //[Fact]
    //public async Task Handle_ReturnsOk_WithRaceId_OnSuccess()
    //{
    //    // Arrange
    //    var handler = CreateHandler();

    //    var dto = new RaceDto { Name = "Elf" }; // uprav dle skutečného DTO
    //    var cmd = new CreatedRaceCommand(dto);

    //    var mapped = new Race { Id = 123, Name = "Elf" };

    //    _mapper.Setup(m => m.Map<Race>(dto)).Returns(mapped);
    //    _repository.Setup(r => r.AddAsync(mapped, It.IsAny<CancellationToken>()))
    //               .Returns(Task.CompletedTask);

    //    var cts = new CancellationTokenSource();

    //    // Act
    //    var result = await handler.Handle(cmd, cts.Token);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess, "Response musí být úspěšný");
    //    Assert.Equal(123, result.Data);
    //    Assert.Equal(LogEventIds.CommandCreated, result.EventId);
    //    Assert.Equal("test-trace-id", result.TraceId);
    //    Assert.True(result.ElapsedMs >= 0);

    //    _mapper.Verify(m => m.Map<Race>(dto), Times.Once);
    //    _repository.Verify(r => r.AddAsync(mapped, It.IsAny<CancellationToken>()), Times.Once);
    //}

    //[Fact]
    //public async Task Handle_ReturnsCanceled_WhenTokenIsCanceled()
    //{
    //    // Arrange
    //    var handler = CreateHandler();

    //    var dto = new RaceDto { Name = "Orc" };
    //    var cmd = new CreatedRaceCommand(dto);

    //    var cts = new CancellationTokenSource();
    //    cts.Cancel();

    //    // Act
    //    var result = await handler.Handle(cmd, cts.Token);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsCanceled, "Response musí indikovat zrušení");
    //    Assert.Equal(LogEventIds.CommandCanceled, result.EventId);
    //    Assert.Equal("test-trace-id", result.TraceId);

    //    _repository.Verify(r => r.AddAsync(It.IsAny<Race>(), It.IsAny<CancellationToken>()), Times.Never);
    //    _mapper.Verify(m => m.Map<Race>(It.IsAny<RaceDto>()), Times.Never);
    //}

    //[Fact]
    //public async Task Handle_ReturnsFail_WhenRepositoryThrows()
    //{
    //    // Arrange
    //    var handler = CreateHandler();

    //    var dto = new RaceDto { Name = "Human" };
    //    var cmd = new CreatedRaceCommand(dto);

    //    var mapped = new Race { Id = 5, Name = "Human" };

    //    _mapper.Setup(m => m.Map<Race>(dto)).Returns(mapped);
    //    _repository.Setup(r => r.AddAsync(mapped, It.IsAny<CancellationToken>()))
    //               .ThrowsAsync(new InvalidOperationException("db error"));

    //    var cts = new CancellationTokenSource();

    //    // Act
    //    var result = await handler.Handle(cmd, cts.Token);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsFailed, "Response musí indikovat chybu");
    //    Assert.Equal(LogEventIds.CommandFailed, result.EventId);
    //    Assert.Equal("test-trace-id", result.TraceId);
    //    Assert.Contains("db error", result.Error, StringComparison.OrdinalIgnoreCase);

    //    _mapper.Verify(m => m.Map<Race>(dto), Times.Once);
    //    _repository.Verify(r => r.AddAsync(mapped, It.IsAny<CancellationToken>()), Times.Once);
    //}
}

// Poznámka:
// - Pokud se názvy typů/namespace liší (např. 'HoL.Application' vs 'HoL.Aplication'), uprav using/directive tak, aby odpovídaly projektu.
// - 'RaceDto', 'Response<T>', 'LogEventIds', 'IRaceRepository', 'Race' a 'CreatedRaceCommand' musí být referencovány z hlavního projektu.


