namespace SerilogEcsLogging.Logging;

public class EcsEvent : IEcsEvent
{
    public string EventAction { get; }
    
    public string EventKind { get; }
    
    public IList<string>? Tags { get; }

    public bool? EventOutcome { get; set; }
    
    public TimeSpan? EventDuration { get; set; }

    public string? EventMessage { get; set; }

    public object? EventData { get; set; }
    
    public string? TransactionId { get; set; }
    
    public string? TraceId { get; set; }

    public EcsEvent(string eventAction, string eventKind = Elastic.CommonSchema.EventKind.Event, IList<string>? tags = null, bool? eventOutcome = null, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null)
    {
        EventAction = eventAction;
        EventKind = eventKind;
        Tags = tags;
        EventOutcome = eventOutcome;
        EventDuration = eventDuration;
        EventMessage = message;
        EventData = eventData;
        TransactionId = transactionId;
        TraceId = traceId;
    }

    public EcsEvent Outcome(bool eventOutcome)
    {
        EventOutcome = eventOutcome;
        return this;
    }
    
    public EcsEvent Duration(TimeSpan duration)
    {
        EventDuration = duration;
        return this;
    }
    
    public EcsEvent Message(string? message)
    {
        EventMessage = message;
        return this;
    }
    
    public EcsEvent Data(object? data)
    {
        EventData = data;
        return this;
    }
}