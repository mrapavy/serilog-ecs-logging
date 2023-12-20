using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Elastic.CommonSchema;
using Microsoft.Extensions.Logging;

namespace SerilogEcsLogging.Logging;

// TODO Consider refactoring using Elastic.Apm API: https://www.elastic.co/guide/en/apm/agent/dotnet/current/public-api.html

[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public static class LoggerExtensions
{
    public const string EventData = EcsDocument.DataFieldName;
    public const string Tags = "Tags";

    #region Events

    public static void LogEvent(this ILogger logger, LogLevel logLevel, Exception? exception, IEcsEvent ecsEvent, DateTime? eventStart = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        string outcome = EventOutcome.Unknown;
        if (exception == null)
        {
            if (ecsEvent.EventOutcome != null)
            {
                outcome = ecsEvent.EventOutcome.Value ? EventOutcome.Success : EventOutcome.Failure;
            }
        }
        else
        {
            outcome = EventOutcome.Failure;
        }
        
        using (logger.BeginScope(@$"{{{LogTemplateProperties.LogOriginFileName}}} {{{LogTemplateProperties.LogOriginFileLine}}} {{{LogTemplateProperties.LogOriginFunction}}} 
                                                {{{LogTemplateProperties.EventSeverity}}} {{{LogTemplateProperties.EventAction}}} {{{LogTemplateProperties.EventId}}} {{{LogTemplateProperties.EventKind}}}
                                                {{{LogTemplateProperties.EventDuration}}} {{{LogTemplateProperties.EventOutcome}}} {{{LogTemplateProperties.EventStart}}} {{{LogTemplateProperties.EventModule}}}
                                                {{{LogTemplateProperties.ErrorCode}}} {{{LogTemplateProperties.ErrorMessage}}} {{{LogTemplateProperties.ErrorStackTrace}}} {{{LogTemplateProperties.TransactionId}}}
                                                {{{LogTemplateProperties.TraceId}}} {{{LogTemplateProperties.ServiceState}}} {{@{EventData}}} {{@{Tags}}}",
                   sourceFilePath, sourceLineNumber, memberName, LogLevel.None - logLevel, ecsEvent.EventAction, ecsEvent.EventId, ecsEvent.EventKind, (long?)(ecsEvent.EventDuration?.TotalMilliseconds * 1000000) /*nanoseconds*/, outcome,
                             eventStart, ecsEvent.EventModule, ecsEvent.ErrorCode, ecsEvent.ErrorMessage, ecsEvent.ErrorException, ecsEvent.TransactionId, ecsEvent.TraceId, ecsEvent.ServiceState?.ToString(), ecsEvent.EventData, ecsEvent.Tags))
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