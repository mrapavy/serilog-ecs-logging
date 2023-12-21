namespace SerilogEcsLogging.Logging;

public class EcsEvent : IEcsEvent
{
    public string EventAction { get; }

    public string EventId { get; }

    public string EventKind { get; }
    
    public ISet<string> Tags { get; }

    public bool? EventOutcome { get; set; }
    
    public TimeSpan? EventDuration { get; set; }

    public string? EventMessage { get; set; }

    public object? EventData { get; set; }
    
    public string? EventModule { get; set; }
    
    public string? ErrorCode { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public string? ErrorException { get; set; }

    public string? TransactionId { get; set; }
    
    public string? TraceId { get; set; }

    public bool? ServiceState { get; set; }

    public EcsDocument ToEcsDocument() => new EcsDocument(this);

    public EcsEvent(string eventAction, string eventKind = Logging.EventKind.Event, ISet<string>? tags = null, bool? eventOutcome = null, TimeSpan? eventDuration = null, string? message = null, object? eventData = null, string? transactionId = null, string? traceId = null, string? eventId = null, string? eventModule = null, string? errorCode = null, string? errorMessage = null, string? errorException = null, bool? serviceState = null)
    {
        EventAction = eventAction;
        EventId = eventId ?? Guid.NewGuid().ToString();
        EventKind = eventKind;
        Tags = tags ?? new HashSet<string>();
        EventOutcome = eventOutcome;
        EventDuration = eventDuration;
        EventMessage = message;
        EventData = eventData;
        TransactionId = transactionId;
        TraceId = traceId;
        EventModule = eventModule;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        ErrorException = errorException;
        ServiceState = serviceState;
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

    public EcsEvent Error(string message, string? code = null, string? exception = null)
    {
        if (code != null)
        {
            ErrorCode = code;
        }

        if (exception != null)
        {
            ErrorException = exception;
        }

        ErrorMessage = message;
        return this;
    }
}