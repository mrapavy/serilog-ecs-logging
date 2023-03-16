namespace SerilogEcsLogging.Logging;

public class AlertBase : EcsEvent
{
    public AlertBase(string eventAction, bool eventOutcome = false, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null) 
        : base(eventAction, Elastic.CommonSchema.EventKind.Alert, new []{"Error"}, eventOutcome, eventDuration, message, eventData, transactionId, traceId)
    {
    }
}