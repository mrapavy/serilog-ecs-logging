namespace SerilogEcsLogging.Logging;

public class MetricBase : EcsEvent
{
    public MetricBase(string eventAction, bool eventOutcome = true, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null) 
        : base(eventAction, Elastic.CommonSchema.EventKind.Metric, new []{"Metric"}, eventOutcome, eventDuration, message, eventData, transactionId, traceId)
    {
    }
}