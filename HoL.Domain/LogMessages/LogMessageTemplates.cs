namespace HoL.Domain.LogMessages
{
    /// <summary>
    /// Centralized repository of log message templates for generic repository operations.
    /// All messages include {Source} placeholder for identifying the originating component.
    /// </summary>
    public static class LogMessageTemplates
    {
        public static class Adding
        {
            private const string AddingEntityMessage =              "{Source} -> Adding new {EntityType} entity";
            private const string EntityAddedSuccessfullyMessage =   "{Source} -> Successfully added {EntityType} entity";

            /// <summary>
            /// Pattern: "{Source} -> Adding new {EntityType} entity"
            /// </summary>
            public static string AddingEntity(string source, string entityType) =>
                AddingEntityMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);
            /// <summary>
            /// Pattern: "{Source} -> Successfully added {EntityType} entity"
            /// </summary>
            public static string EntityAddedSuccessfully(string source, string entityType) =>
                EntityAddedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);
        }

        public static class Updating
        {
            private const string UpdatingEntityMessage =                    "{Source} -> Updating {EntityType} entity";
            private const string UpdatingEntityWithIdMessage =              "{Source} -> Updating {EntityType} entity with ID {Id}";
            private const string EntityUpdatedSuccessfullyMessage =         "{Source} -> Successfully updated {EntityType} entity";
            private const string EntityUpdatedSuccessfullyWithIdMessage =   "{Source} -> Successfully updated {EntityType} entity with Id: {Id}";

            /// <summary>
            /// Pattern: "{Source} -> Updating {EntityType} entity"
            /// </summary>
            public static string UpdatingEntity(string source, string entityType) =>
                UpdatingEntityMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);

            /// <summary>
            /// Pattern: "{Source} -> Successfully updated {EntityType} entity"
            /// </summary>
            public static string EntityUpdatedSuccessfully(string source, string entityType) =>
                EntityUpdatedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);

            /// <summary>
            /// Pattern: "{Source} -> Updating {EntityType} with ID {Id}"
            /// </summary>
            public static string UpdatingEntityWithId(string source, string entityType, object id) =>
                UpdatingEntityWithIdMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Successfully updated {EntityType} entity with Id: {Id}"
            /// </summary>
            public static string EntityUpdatedSuccessfullyWithId(string source, string entityType, object id) =>
                EntityUpdatedSuccessfullyWithIdMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);
        }

        public static class Deleting
        {
            private const string EntityNotFoundForDeletionMessage = "{Source} -> {EntityType} with ID: {Id} not found for deletion";
            private const string EntityDeletedSuccessfullyMessage = "{Source} -> Successfully deleted {EntityType} with ID: {Id}";
            private const string DeletingEntityInfoMessage =        "{Source} -> Deleting {EntityType} with ID {Id}";


            /// <summary>
            /// Pattern: "{Source} -> {EntityType} with ID: {Id} not found for deletion"
            /// </summary>
            public static string EntityNotFoundForDeletion(string source, string entityType, object id) =>
                EntityNotFoundForDeletionMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Successfully deleted {EntityType} with ID: {Id}"
            /// </summary>
            public static string EntityDeletedSuccessfully(string source, string entityType, object id) =>
                EntityDeletedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Deleting {EntityType} with ID {Id}"
            /// </summary>
            public static string DeletingEntityInfo(string source, string entityType, object id) =>
                DeletingEntityInfoMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

        }

        public static class Creating
        {
            private const string CreatingEntityWithNameMessage =    "{Source} -> Creating new {EntityType} with Name:{EntityName}";
            private const string CreatingEntityMessage =            "{Source} -> Creating new {EntityType}";
            private const string EntityCreatedSuccessfullyMessage = "{Source} -> {EntityType} created successfully";

            /// <summary>
            /// Pattern: "{Source} -> Creating new {EntityType} with Name:{EntityName}"
            /// </summary>
            public static string CreatingEntityWithName(string source, string entityType, string entityName) =>
                CreatingEntityWithNameMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{EntityName}", entityName);

            /// <summary>
            /// Pattern: "{Source} -> Creating new {EntityType}"
            /// </summary>
            public static string CreatingEntity(string source, string entityType) =>
                CreatingEntityMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);

            /// <summary>
            /// Pattern: "{Source} -> {EntityType} created successfully"
            /// </summary>
            public static string EntityCreatedSuccessfully(string source, string entityType) =>
                EntityCreatedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);
        }

        public static class Existence
        {
            private const string EntityExistsMessage =       "{Source} -> {EntityType} with ID: {Id} exists";
            private const string EntityDoesNotExistMessage = "{Source} -> {EntityType} with ID: {Id} does not exist";


            /// <summary>
            /// Pattern: "{Source} -> {EntityType} with ID: {Id} exists"
            /// </summary>
            public static string EntityExists(string source, string entityType, object id) =>
                EntityExistsMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> {EntityType} with ID: {Id} does not exist"
            /// </summary>
            public static string EntityDoesNotExist(string source, string entityType, object id) =>
                EntityDoesNotExistMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);
        }

        public static class GetById
        {
            private const string LookingForEntityByIdMessage =          "{Source} -> Looking for {EntityType} with ID: {Id}";
            private const string EntityNotFoundMessage =                "{Source} -> {EntityType} with ID: {Id} not found";
            private const string EntityRetrievedSuccessfullyMessage =   "{Source} -> Successfully retrieved {EntityType} with ID: {Id}";
            private const string InvalidIdMessage =                     "{Source} -> Invalid ID {Id} (must be > 0). Returning null";

            /// <summary>
            /// Pattern: "{Source} -> Looking for {EntityType} with ID: {Id}"
            /// </summary>
            public static string LookingForEntityById(string source, string entityType, object id) =>
                LookingForEntityByIdMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> {EntityType} with ID: {Id} not found"
            /// </summary>
            public static string EntityNotFound(string source, string entityType, object id) =>
                EntityNotFoundMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Successfully retrieved {EntityType} with ID: {Id}"
            /// </summary>
            public static string EntityRetrievedSuccessfully(string source, string entityType, object id) =>
                EntityRetrievedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Invalid ID {Id} (must be > 0). Returning null"
            /// </summary>
            public static string InvalidId(string source, object id) =>
                InvalidIdMessage
                    .Replace("{Source}", source)
                    .Replace("{Id}", id.ToString()!);
        }

        public static class GetByIds
        {
            private const string NullIdsCollectionMessage = "{Source} -> Ids collection is null. Returning empty result";
            private const string ProcessingIdsMessage =     "{Source} -> Processing {RawCount} raw IDs, {NormalizedCount} normalized IDs: {Ids}";
            private const string QueryCompletedMessage =    "{Source} -> Query completed: requestedCount {Requested}, returnedCount {Returned}";
            private const string NoEntitiesFoundMessage =   "{Source} -> Query completed: no {EntityType} found";
            private const string NoQueryRequestedMessage =  "{Source} -> No query requestedCount. Returning empty result";

            /// <summary>
            /// Pattern: "{Source} -> Ids collection is null. Returning empty result"
            /// </summary>
            public static string NullIdsCollection(string source) =>
                NullIdsCollectionMessage
                    .Replace("{Source}", source);

            /// <summary>
            /// Pattern: "{Source} -> Processing {RawCount} raw IDs, {NormalizedCount} normalized IDs: {Ids}"
            /// </summary>
            public static string ProcessingIds(string source, int rawCount, int normalizedCount, string ids) =>
                ProcessingIdsMessage
                    .Replace("{Source}", source)
                    .Replace("{RawCount}", rawCount.ToString())
                    .Replace("{NormalizedCount}", normalizedCount.ToString())
                    .Replace("{Ids}", ids);

            /// <summary>
            /// Pattern: "{Source} -> Query completed: requestedCount {Requested}, returnedCount {Returned}"
            /// </summary>
            public static string QueryCompleted(string source, int requested, int returned) =>
                QueryCompletedMessage
                    .Replace("{Source}", source)
                    .Replace("{Requested}", requested.ToString())
                    .Replace("{Returned}", returned.ToString());

            /// <summary>
            /// Pattern: "{Source} -> Query completed: no {EntityType} found"
            /// </summary>
            public static string NoEntitiesFound(string source, string entityType) =>
                NoEntitiesFoundMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);

            /// <summary>
            /// Pattern: "{Source} -> No query requestedCount. Returning empty result"
            /// </summary>
            public static string NoQueryRequested(string source) =>
                NoQueryRequestedMessage
                    .Replace("{Source}", source);
        }

        public static class GetAll
        {
            private const string RetrievingAllEntitiesMessage =         "{Source} -> Retrieving all {EntityType} entities";
            private const string EntitiesRetrievedSuccessfullyMessage = "{Source} -> Successfully retrieved {Count} {EntityType} entities";

            /// <summary>
            /// Pattern: "{Source} -> Retrieving all {EntityType} entities"
            /// </summary>
            public static string RetrievingAllEntities(string source, string entityType) =>
                RetrievingAllEntitiesMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType);

            /// <summary>
            /// Pattern: "{Source} -> Successfully retrieved {Count} {EntityType} entities"
            /// </summary>
            public static string EntitiesRetrievedSuccessfully(string source, int count, string entityType) =>
                EntitiesRetrievedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{Count}", count.ToString())
                    .Replace("{EntityType}", entityType);

        }

        public static class GetByName
        {
            private const string LookingForEntityByNameMessage =        "{Source} -> Looking for {EntityType} with name: {Name}";
            private const string EntityNotFoundByNameMessage =          "{Source} -> {EntityType} with name: {Name} not found";
            private const string EntityRetrievedSuccessfullyByNameMessage = "{Source} -> Successfully retrieved {EntityType} with name: {Name}";
            private const string ErrorRetrievingEntityByNameMessage = " {Source} -> Error retrieving {EntityType} with name: {Name}";

            /// <summary>
            /// Pattern: "{Source} -> Looking for {EntityType} with name: {Name}"
            /// </summary>
            public static string LookingForEntityByName(string source, string entityType, string name) =>
                LookingForEntityByNameMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Name}", name);

            /// <summary>
            /// Pattern: "{Source} -> {EntityType} with name: {Name} not found"
            /// </summary>
            public static string EntityNotFoundByName(string source, string entityType, string name) =>
                EntityNotFoundByNameMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Name}", name);

            /// <summary>
            /// Pattern: "{Source} -> Successfully retrieved {EntityType} with name: {Name}"
            /// </summary>
            public static string EntityRetrievedSuccessfullyByName(string source, string entityType, string name) =>
                EntityRetrievedSuccessfullyByNameMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Name}", name);

            /// <summary>
            /// Pattern: "{Source} -> Error retrieving {EntityType} with name: {Name}"
            /// </summary>
            public static string ErrorRetrievingEntityByName(string source, string entityType, string name) =>
                ErrorRetrievingEntityByNameMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Name}", name);
        }

        public static class Pagination
        {
            private const string RetrievingPageMessage =            "{Source} -> Retrieving {EntityType} page {Page} with size {Size}";
            private const string PageRetrievedSuccessfullyMessage = "{Source} -> Successfully retrieved {Count} {EntityType} entities for page {Page}";
            private const string RetrievingPageWithSortMessage =    "{Source} -> Retrieving {EntityType} page {Page}, size {Size}, sortBy: {SortBy}, direction: {SortDir}";
            private const string SequenceQueryCompletedMessage =    "{Source} -> Successfully retrieved {Count} {EntityType}(s)";

            /// <summary>
            /// Pattern: "{Source} -> Retrieving {EntityType} page {Page} with size {Size}"
            /// </summary>
            public static string RetrievingPage(string source, string entityType, int page, int size) =>
                RetrievingPageMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Page}", page.ToString())
                    .Replace("{Size}", size.ToString());

            /// <summary>
            /// Pattern: "{Source} -> Successfully retrieved {Count} {EntityType} entities for page {Page}"
            /// </summary>
            public static string PageRetrievedSuccessfully(string source, int count, string entityType, int page) =>
                PageRetrievedSuccessfullyMessage
                    .Replace("{Source}", source)
                    .Replace("{Count}", count.ToString())
                    .Replace("{EntityType}", entityType)
                    .Replace("{Page}", page.ToString());

            /// <summary>
            /// Pattern: "{Source} -> Retrieving {EntityType} page {Page}, size {Size}, sortBy: {SortBy}, direction: {SortDir}"
            /// </summary>
            public static string RetrievingPageWithSort(string source, string entityType, int page, int size, string sortBy, string sortDir) =>
                RetrievingPageWithSortMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Page}", page.ToString())
                    .Replace("{Size}", size.ToString())
                    .Replace("{SortBy}", sortBy)
                    .Replace("{SortDir}", sortDir);

            /// <summary>
            /// Pattern: "{Source} -> Successfully retrieved {Count} {EntityType}(s)"
            /// </summary>
            public static string SequenceQueryCompleted(string source, int count, string entityType) =>
                SequenceQueryCompletedMessage
                    .Replace("{Source}", source)
                    .Replace("{Count}", count.ToString())
                    .Replace("{EntityType}", entityType);
        }

        public static class Exceptions
        {
            private const string OperationCanceledWithIdMessage =   "{Source} -> Operation canceled for {EntityType} with ID {Id}";
            private const string OperationCanceledMessage =         "{Source} -> Operation canceled : Exception";
            private const string UnexpectedErrorWithIdMessage =     "{Source} -> Unexpected error retrieving {EntityType} with ID {Id}: Exception -> {exception}";
            private const string UnexpectedErrorMessage =           "{Source} -> Unexpected error retrieving {EntityType}: Exception -> {exception}";
            private const string ValidationFailedMessage =          "{Source} -> Validation failed: {ValidationMessage}";

            /// <summary>
            /// Pattern: "{Source} -> Operation canceled for {EntityType} with ID {Id}"
            /// </summary>
            public static string OperationCanceledWithId(string source, string entityType, object id) =>
                OperationCanceledWithIdMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!);

            /// <summary>
            /// Pattern: "{Source} -> Operation canceled"
            /// </summary>
            public static string OperationCanceled(string source) =>
                OperationCanceledMessage
                    .Replace("{Source}", source);

            /// <summary>
            /// Pattern: "{Source} -> Unexpected error retrieving {EntityType} with ID {Id}: Exception -> {exception}"
            /// </summary>
            public static string UnexpectedErrorWithId(string source, string entityType, object id, Exception ex) =>
                UnexpectedErrorWithIdMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{Id}", id.ToString()!)
                    .Replace("{exception}",ex.Message);

            /// <summary>
            /// Pattern: "{Source} -> Unexpected error {EntityType}: Exception -> {exception}"
            /// </summary>
            public static string UnexpectedError(string source, string entityType, Exception ex) =>
                UnexpectedErrorMessage
                    .Replace("{Source}", source)
                    .Replace("{EntityType}", entityType)
                    .Replace("{exception}", ex.Message);

            /// <summary>
            /// Pattern: "{Source} -> Validation failed: {ValidationMessage}"
            /// </summary>
            public static string ValidationFailed(string source, string validationMessage) =>
                ValidationFailedMessage
                    .Replace("{Source}", source)
                    .Replace("{ValidationMessage}", validationMessage);
        }

        public static class Generic
        {
            private const string InfoMessage = "{Source} -> {Message}";
            private const string WarningMessage = "{Source} -> {Message}";

            /// <summary>
            /// Pattern: "{Source} -> {Message}"
            /// </summary>
            public static string Info(string source, string message) =>
                InfoMessage
                    .Replace("{Source}", source)
                    .Replace("{Message}", message);

            /// <summary>
            /// Pattern: "{Source} -> {Message}"
            /// </summary>
            public static string Warning(string source, string message) =>
                WarningMessage
                    .Replace("{Source}", source)
                    .Replace("{Message}", message);
        }
    }
}