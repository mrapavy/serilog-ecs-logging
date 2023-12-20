namespace SerilogEcsLogging.Logging;

public class EventBase : EcsEvent
{
    public EventBase(string eventAction, bool? eventOutcome = null, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null, string? eventId = null, string? eventModule = null, bool? serviceState = null) 
        : base(eventAction, Logging.EventKind.Event, null, eventOutcome, eventDuration, message, eventData, transactionId, traceId, eventId, eventModule, serviceState: serviceState)
    {
    }
}