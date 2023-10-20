namespace SerilogEcsLogging.Logging;

public interface IEcsEvent
{
    string EventAction { get; }

    string EventKind { get; }
    
    IList<string>? Tags { get; }

    bool? EventOutcome { get; set; }

    TimeSpan? EventDuration { get; set; }

    string? EventMessage { get; set; }
    
    object? EventData { get; set; }
    
    string? TransactionId { get; set; }

    string? TraceId { get; set; }
}