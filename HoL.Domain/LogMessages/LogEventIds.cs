using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Enums.Logging;
using Microsoft.Extensions.Logging;

namespace HoL.Domain.LogMessages
{
    /// <summary>
    /// Centralizovaný registr EventId pro strukturované logování
    /// </summary>
    public static class LogEventIds
    {
        
        // ========== QUERIES ==========
        public static readonly EventId QueryHandled = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Query,
            LogLevelCodeType.Information,
            EventVariantType.QueryHandled);

        public static readonly EventId QueryNotFoundHandled = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Query,
            LogLevelCodeType.Warning,
            EventVariantType.QueryNotFound);

        public static readonly EventId QueryInvalidHandled = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Query,
            LogLevelCodeType.Warning,
            EventVariantType.QueryInvalid);

        public static readonly EventId QueryFailed = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Query,
            LogLevelCodeType.Error,
            EventVariantType.UnhandledException);

        public static readonly EventId QueryCenceled= LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Query,
            LogLevelCodeType.Error,
            EventVariantType.TaskAborted);

        // ========== COMMANDS ==========
        public static readonly EventId CommandCanceled = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Command,
            LogLevelCodeType.Error,
            EventVariantType.TaskAborted);

        public static readonly EventId CommandFailed = LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Command,
            LogLevelCodeType.Error,
            EventVariantType.UnhandledException);

        public static readonly EventId CommandCreated= LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Command,
            LogLevelCodeType.Information,
            EventVariantType.EntityCreated);

        public static readonly EventId CommandUpdated= LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Command,
            LogLevelCodeType.Information,
            EventVariantType.EntityUpdated);

        public static readonly EventId CommandDeleted= LogIdFactory.Create(
            ProjectLayerType.Application,
            OperationType.Command,
            LogLevelCodeType.Information,
            EventVariantType.EntityDeleted);

    }
}
