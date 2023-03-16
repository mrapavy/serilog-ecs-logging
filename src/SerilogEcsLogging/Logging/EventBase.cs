namespace SerilogEcsLogging.Logging;

public class EventBase : EcsEvent
{
    public EventBase(string eventAction, bool? eventOutcome = null, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null) 
        : base(eventAction, Elastic.CommonSchema.EventKind.Event, null, eventOutcome, eventDuration, message, eventData, transactionId, traceId)
    {
    }
}