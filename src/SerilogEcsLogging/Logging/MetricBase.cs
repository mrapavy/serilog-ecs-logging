namespace SerilogEcsLogging.Logging;

public class MetricBase : EcsEvent
{
    public MetricBase(string eventAction, bool eventOutcome = true, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null, string? eventId = null, string? eventModule = null) 
        : base(eventAction, Logging.EventKind.Metric, new HashSet<string> {"metric"}, eventOutcome, eventDuration, message, eventData, transactionId, traceId, eventId, eventModule)
    {
    }
}