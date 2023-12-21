namespace SerilogEcsLogging.Logging;

public interface IEcsEvent
{
    string EventAction { get; }

    string EventId { get; }

    string EventKind { get; }
    
    ISet<string>? Tags { get; }

    bool? EventOutcome { get; set; }

    TimeSpan? EventDuration { get; set; }

    string? EventMessage { get; set; }
    
    object? EventData { get; set; }

    string? EventModule { get; set; }

    string? ErrorCode { get; set; }

    string? ErrorMessage { get; set; }

    string? ErrorException { get; set; }

    string? TransactionId { get; set; }

    string? TraceId { get; set; }
    
    bool? ServiceState { get; set; }

    Elastic.CommonSchema.EcsDocument ToEcsDocument();
}