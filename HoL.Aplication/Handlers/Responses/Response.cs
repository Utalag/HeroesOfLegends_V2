using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Responses
{
    public class Response<T>
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public List<string> ValidationErrors { get; init; } = new();
        public T? Data { get; init; }

        // Tracing / logging
        public string? TraceId { get; init; }
        public EventId? EventId { get; init; }

        // Metadata
        public int StatusCode { get; init; }
        public long ElapsedMs { get; init; }

        #region Response Factory Methods
        // Factory metody
        public static Response<T> Ok(
            T data,
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                Success = true,
                Data = data,
                StatusCode = 200,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response<T> NoContent(
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                ErrorMessage = "Entity not found.",
                Success = true,
                Data = default,
                StatusCode = 204,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response<T> Fail(
            string error,
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 500,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = error ?? "An error occurred.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response<T> ValidationFailed(
            IEnumerable<string> errors,
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 422,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ValidationErrors = (errors ?? Array.Empty<string>()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList(),
                ErrorMessage = "Validation failed.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response<T> Canceled(
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 499, // 408 Request Timeout (alternativně 499 pokud preferujete klientem zrušený požadavek)
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = "Operation was canceled.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId,
            };
        #endregion

    }

    public class Response
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public List<string> ValidationErrors { get; init; } = new();

        // Tracing / logging
        public string? TraceId { get; init; }
        public EventId? EventId { get; init; }

        // Metadata
        public int StatusCode { get; init; }
        public long ElapsedMs { get; init; }


        #region Response Factory Methods

        public static Response Ok(
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                Success = true,
                StatusCode = 200,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response NoContent(
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                ErrorMessage = "Entity not found.",
                Success = true,
                StatusCode = 204,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response Fail(
            string error,
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 500,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = error ?? "An error occurred.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response ValidationFailed(
            IEnumerable<string> errors,
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 422,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ValidationErrors = (errors ?? Array.Empty<string>()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList(),
                ErrorMessage = "Validation failed.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };

        public static Response Canceled(
            EventId? eventId = null,
            string? traceId = null,
            int statusCode = 499,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = "Operation was canceled.",
                StatusCode = statusCode,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                EventId = eventId
            };
        #endregion
    }
}
