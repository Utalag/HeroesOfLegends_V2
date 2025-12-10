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

        // Repository operace
        RepositoryRead = 13,
        RepositoryAdd = 20,
        RepositoryUpdate = 21,
        RepositoryDelete = 22,

        // Validace
        ValidationFailed = 30,
        ValidationWarning = 31,

        // Chyby
        UnhandledException = 40,
        TimeoutOccurred = 41,
        ExternalServiceError = 42,
        TaskAborted = 43,

        // Bezpečnost
        UnauthorizedAccess = 50,
        ForbiddenOperation = 51,
        AuthenticationFailed = 52,

        // Integrace / externí volání
        ExternalRequestSent = 60,
        ExternalResponseReceived = 61,

        // Systémové události
        Startup = 70,
        Shutdown = 71,
        ConfigurationLoaded = 72

    }
}
