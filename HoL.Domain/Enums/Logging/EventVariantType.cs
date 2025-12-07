using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.Enums.Logging
{
    public enum EventVariantType
    {
        /// Command / změny entit
        EntityCreated = 1,
        EntityUpdated = 2,
        EntityDeleted = 3,

        // Query / dotazy
        QueryHandled = 10,
        QueryNotFound = 11,
        QueryInvalid = 12,

        // Validace
        ValidationFailed = 20,
        ValidationWarning = 21,

        // Chyby
        UnhandledException = 30,
        TimeoutOccurred = 31,
        ExternalServiceError = 32,
        TaskAborted = 33,

        // Bezpečnost
        UnauthorizedAccess = 40,
        ForbiddenOperation = 41,
        AuthenticationFailed = 42,

        // Integrace / externí volání
        ExternalRequestSent = 50,
        ExternalResponseReceived = 51,

        // Systémové události
        Startup = 60,
        Shutdown = 61,
        ConfigurationLoaded = 62

    }
}
