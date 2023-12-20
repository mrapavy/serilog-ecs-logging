namespace SerilogEcsLogging.Logging;

public class AlertBase : EcsEvent
{
    public AlertBase(string eventAction, bool eventOutcome = false, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null,
                     string? eventId = null, string? eventModule = null, string? errorCode = null, string? errorMessage = null, string? errorException = null) 
        : base(eventAction, Logging.EventKind.Alert, new HashSet<string> {"alert"}, eventOutcome, eventDuration, message, eventData, transactionId, traceId, eventId, eventModule, errorCode, errorMessage, errorException)
    {
    }
}