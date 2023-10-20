using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace SerilogEcsLogging.Logging;

// TODO Consider refactoring using Elastic.Apm API: https://www.elastic.co/guide/en/apm/agent/dotnet/current/public-api.html

[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public static class LoggerExtensions
{
    public const string LogFilename = "LogOriginFileName";
    public const string LogLineNumber = "LogOriginLineNumber";
    public const string LogMethodName = "LogOriginMethodName";
    public const string EventActionSeverity = "ActionSeverity";
    public const string EventActionName = "ActionName";
    public const string EventActionKind = "ActionKind";
    public const string EventDuration = "ElapsedMilliseconds";
    public const string EventOutcome = "ActionOutcome";
    public const string EventStart = "ActionStart";
    public const string EventData = Base.DataFieldName;
    public const string TransactionId = "ElasticApmTransactionId";
    public const string TraceId = "ElasticApmTraceId";
    public const string Tags = "Tags";

    #region Events

    public static void LogEvent(this ILogger logger, LogLevel logLevel, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        var outcome = Elastic.CommonSchema.EventOutcome.Unknown;
        if (exception == null)
        {
            if (ecsEvent.EventOutcome != null)
            {
                outcome = ecsEvent.EventOutcome.Value ? Elastic.CommonSchema.EventOutcome.Success : Elastic.CommonSchema.EventOutcome.Failure;
            }
        }
        else
        {
            outcome = Elastic.CommonSchema.EventOutcome.Failure;
        }
        
        using (logger.BeginScope($"{{{LogFilename}}} {{{LogLineNumber}}} {{{LogMethodName}}} {{{EventActionSeverity}}} {{{EventActionName}}} {{{EventActionKind}}} {{{EventDuration}}} {{{EventOutcome}}} {{{EventStart}}} {{@{EventData}}} {{@{Tags}}}", sourceFilePath, sourceLineNumber, memberName, LogLevel.None - logLevel, ecsEvent.EventAction, ecsEvent.EventKind, ecsEvent.EventDuration, outcome, eventStart, ecsEvent.EventData, ecsEvent.Tags))
        {
            logger.Log(logLevel, exception, ecsEvent.EventMessage);
        }
    }
    
    public static void LogEvent(this ILogger logger, LogLevel logLevel, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(logLevel, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Critical

    public static void LogCriticalEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Critical, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogCriticalEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Critical, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Error
    
    public static void LogErrorEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Error, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogErrorEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Error, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Warning
    
    public static void LogWarningEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Warning, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogWarningEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Warning, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Information
    
    public static void LogInformationEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Information, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogInformationEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Information, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Debug
    
    public static void LogDebugEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Debug, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogDebugEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Debug, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    // Trace
    
    public static void LogTraceEvent(this ILogger logger, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Trace, exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }
    
    public static void LogTraceEvent(this ILogger logger, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogEvent(LogLevel.Trace, null, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
    }

    #endregion

    #region Timed Events

    public static TimedEvent BeginTimedEvent(this ILogger logger, IEcsEvent ecsEvent, LogLevel levelSuccess = LogLevel.Information, LogLevel levelFailure = LogLevel.Error, bool autoComplete = true, string? messageSuccess = null, string? messageFailure = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new TimedEvent(logger, ecsEvent, levelSuccess, levelFailure, autoComplete, messageSuccess, messageFailure, memberName, sourceFilePath, sourceLineNumber);
    }

    #endregion
}