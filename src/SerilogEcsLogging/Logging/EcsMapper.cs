using Elastic.CommonSchema;
using Serilog.Events;

namespace SerilogEcsLogging.Logging;

public static class EcsMapper
{
    public static Elastic.CommonSchema.Base MapLogEvent(Elastic.CommonSchema.Base ecsEvent, LogEvent logEvent)
    {
        var result = ecsEvent;
        
        // Add custom event data
        if (ecsEvent.Metadata.ContainsKey(Base.DataFieldName))
        {
            result = new Base(ecsEvent, ecsEvent.Metadata[Base.DataFieldName]);
        }

        // Add properties currently missing in EcsTextFormatter (as of Elastic.CommonSchema.Serilog 1.5.3)
        
        // Event.Outcome
        if (logEvent.Properties.TryGetValue(LoggerExtensions.EventOutcome, out var actionOutcome) && actionOutcome is ScalarValue)
        {
            result.Event.Outcome = actionOutcome.ToString().Trim('"');
        }
        else
        {
            result.Event.Outcome = EventOutcome.Unknown;
        }
        
        // Event.Duration
        if (logEvent.Properties.TryGetValue(LoggerExtensions.EventDuration, out var actionDuration) && actionDuration is ScalarValue && long.TryParse(actionDuration.ToString(), out var actionDurationMs))
        {
            result.Event.Duration = actionDurationMs * 1000000; // Event.Duration is specified as duration in nanoseconds
        }
        
        // Event.Start
        if (logEvent.Properties.TryGetValue(LoggerExtensions.EventStart, out var actionStart) && actionStart is ScalarValue && DateTime.TryParse(actionStart.ToString(), out var actionStartDateTime))
        {
            result.Event.Start = actionStartDateTime;
        }

        // Service.Name
        if (logEvent.Properties.TryGetValue("AssemblyName", out var assemblyName) && assemblyName is ScalarValue)
        {
            result.Service ??= new Service();
            result.Service.Name = assemblyName.ToString().Trim('"');
        }
        
        // Service.Version
        if (logEvent.Properties.TryGetValue("AssemblyVersion", out var assemblyVersion) && assemblyVersion is ScalarValue)
        {
            result.Service ??= new Service();
            result.Service.Version = assemblyVersion.ToString().Trim('"');
        }

        // Service.Environment
        /* TODO Add when Service.Environment field gets added to Elastic.CommonSchema
        if (logEvent.Properties.TryGetValue("EnvironmentName", out var environmentName) && environmentName is ScalarValue)
        {
            result.Service ??= new Service();
            result.Service.Environment = environmentName.ToString().Trim('"');;
        }
        */
        
        // Transaction.Id
        if (logEvent.Properties.TryGetValue(LoggerExtensions.TransactionId, out var transactionId) && transactionId is ScalarValue)
        {
            result.Transaction ??= new Transaction();
            result.Transaction.Id = transactionId.ToString().Trim('"');
        }
        else if (logEvent.Properties.TryGetValue("CorrelationId", out var correlationId) && correlationId is ScalarValue)
        {
            result.Transaction ??= new Transaction();
            result.Transaction.Id = correlationId.ToString().Trim('"');
        } 
        
        // Trace.Id
        if (logEvent.Properties.TryGetValue(LoggerExtensions.TraceId, out var traceId) && traceId is ScalarValue)
        {
            result.Trace ??= new Trace();
            result.Trace.Id = traceId.ToString().Trim('"');
        }

        // Log.Origin
        if (logEvent.Properties.TryGetValue(LoggerExtensions.LogFilename, out var fileName) && fileName is ScalarValue)
        {
            result.Log ??= new Log();
            result.Log.Origin ??= new LogOrigin();
            result.Log.Origin.File ??= new OriginFile();
            result.Log.Origin.File.Name = Path.GetFileName(fileName.ToString().Trim('"'));
        }
        
        if (logEvent.Properties.TryGetValue(LoggerExtensions.LogLineNumber, out var lineNumber) && lineNumber is ScalarValue && int.TryParse(lineNumber.ToString(), out var lineNumberInt))
        {
            result.Log ??= new Log();
            result.Log.Origin ??= new LogOrigin();
            result.Log.Origin.File ??= new OriginFile();
            result.Log.Origin.File.Line = lineNumberInt;
        }
        
        if (logEvent.Properties.TryGetValue(LoggerExtensions.LogMethodName, out var methodName) && methodName is ScalarValue)
        {
            result.Log ??= new Log();
            result.Log.Origin ??= new LogOrigin();
            result.Log.Origin.Function = methodName.ToString().Trim('"');
        }
        
        // Tags
        if (ecsEvent.Metadata.ContainsKey(LoggerExtensions.Tags) && ecsEvent.Metadata[LoggerExtensions.Tags] is object[] tags)
        {
            var tagsStrings = Array.ConvertAll(tags, o => o.ToString());
            result.Tags = result.Tags != null ? result.Tags.Concat(tagsStrings).ToArray() : tagsStrings;
        }

        if (result.Tags != null)
        {
            Array.Sort(result.Tags);
        }

        // Get rid of the Metadata field
        result.Metadata = null;
        return result;
    }
}