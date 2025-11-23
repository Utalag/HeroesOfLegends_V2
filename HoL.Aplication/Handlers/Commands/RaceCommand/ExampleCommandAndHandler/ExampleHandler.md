using AutoMapper;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Interfaces;
using HoL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.ExampleCommandAndHandler
{
    /// <summary>
    /// Handler pro zpracování ... (popište účel handleru)
    /// </summary>
    public class ExampleHandler : IRequestHandler<ExampleCommand, ExampleDto>
    {
        private readonly IExampleRepository _repository; // ... upravte typ repository
        private readonly IMapper _mapper;
        private readonly ILogger<ExampleHandler> _logger;

        public ExampleHandler(
            IExampleRepository repository, // ... upravte typ repository
            IMapper mapper, 
            ILogger<ExampleHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExampleDto> Handle(ExampleCommand request, CancellationToken cancellationToken)
        {
            // 1. Validace vstupu
            if (request is null)
            {
                _logger.LogError("Request is null");
                throw new ArgumentNullException(nameof(request));
            }

            // ... přidejte další validace podle potřeby (např. request.Id > 0)
            
            _logger.LogInformation("Handling ExampleCommand for Id: {Id}", request.Id); // ... upravte logované parametry

            try
            {
                // 2. Načtení dat z repository
                var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
                
                // 3. Kontrola existence entity
                if (entity == null)
                {
                    _logger.LogWarning("Entity with Id: {Id} not found", request.Id);
                    throw new KeyNotFoundException($"Entity with Id {request.Id} not found."); // ... nebo vlastní NotFoundException
                }

                // 4. Mapování na DTO (nebo zpracování business logiky)
                var dto = _mapper.Map<ExampleDto>(entity);

                // 5. Úspěšné logování
                _logger.LogInformation("Successfully handled ExampleCommand for Id: {Id}", request.Id);

                return dto;
            }
            catch (Exception ex) when (ex is not ArgumentNullException && ex is not KeyNotFoundException)
            {
                // 6. Logování neočekávaných chyb
                _logger.LogError(ex, "Error handling ExampleCommand for Id: {Id}", request.Id);
                throw; // Re-throw pro vyšší vrstvy (např. API pipeline)
            }
        }
    }
}
