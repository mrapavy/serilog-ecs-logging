namespace SerilogEcsLogging.Logging;

public static class EventKind
{
    public const string Alert = "alert";
    public const string Event = "event";
    public const string Metric = "metric";
    public const string State = "state";
    public const string PipelineError = "pipeline_error";
    public const string Signal = "signal";
}