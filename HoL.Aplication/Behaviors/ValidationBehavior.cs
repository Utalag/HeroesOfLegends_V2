using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HoL.Aplication.Behaviors
{
    /// <summary>
    /// MediatR Pipeline Behavior pro automatickou validaci pomocí FluentValidation.
    /// Spustí se před každým handlerem a zkontroluje validitu requestu.
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            // Pokud nejsou žádné validátory, pokračuj dál
            if (!_validators.Any())
            {
                _logger.LogDebug("No validators found for {RequestName}", requestName);
                return await next();
            }

            _logger.LogDebug("Validating {RequestName} with {ValidatorCount} validators", 
                requestName, _validators.Count());

            // Vytvoř kontext pro validaci
            var context = new ValidationContext<TRequest>(request);

            // Spusť všechny validátory paralelně
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Zkontroluj jestli jsou nějaké chyby
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // Pokud jsou chyby, vyhoď ValidationException
            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation failed for {RequestName}. {ErrorCount} validation errors: {Errors}",
                    requestName,
                    failures.Count,
                    string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}")));

                throw new ValidationException(failures);
            }

            _logger.LogDebug("Validation successful for {RequestName}", requestName);

            // Validace proběhla úspěšně, pokračuj k handleru
            return await next();
        }
    }
}
