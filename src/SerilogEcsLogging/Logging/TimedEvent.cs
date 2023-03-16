using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace SerilogEcsLogging.Logging;

[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public class TimedEvent : IDisposable
{
    private readonly ILogger logger;
    private readonly LogLevel levelSuccess;
    private readonly LogLevel levelFailure;
    private readonly Stopwatch sw = Stopwatch.StartNew();
    private readonly string? memberName;
    private readonly string? sourceFilePath;
    private readonly int sourceLineNumber;
    private readonly DateTime eventStart = DateTime.Now;
    private IEcsEvent ecsEvent;
    private Exception? exception;
    private string? successMessage;
    private string? failureMessage;
    private bool success;

    public TimedEvent(ILogger logger, IEcsEvent ecsEvent, LogLevel levelSuccess = LogLevel.Information, LogLevel levelFailure = LogLevel.Error, bool autoComplete = true, string? successMessage = null, string? failureMessage = null, [CallerMemberName] string? memberName = null, [CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
    {
        this.logger = logger;
        this.ecsEvent = ecsEvent;
        this.levelSuccess = levelSuccess;
        this.levelFailure = levelFailure;
        this.memberName = memberName;
        this.sourceFilePath = sourceFilePath;
        this.sourceLineNumber = sourceLineNumber;
        this.successMessage = successMessage;
        this.failureMessage = failureMessage;
        success = autoComplete;
    }

    public bool Success => success = success && Marshal.GetExceptionPointers() == IntPtr.Zero;
    
    public string? Message => string.IsNullOrEmpty(ecsEvent.EventMessage) ? (Success ? successMessage : failureMessage) : ecsEvent.EventMessage;

    public TimeSpan Duration => sw.Elapsed;

    public void Complete(object? data = null, string? message = null)
    {
        success = true;
        ecsEvent.EventData = data;
        if (message != null)
        {
            successMessage = message;
        }
    }
    
    public void Complete(IEcsEvent @event)
    {
        success = true;
        ecsEvent = @event;
    }

    public void Fail(Exception? e = null, object? data = null, string? message = null)
    {
        success = false;
        exception = e;
        ecsEvent.EventData = data;
        if (message != null)
        {
            failureMessage = message;
        }
    }
    
    public void Fail(IEcsEvent @event, Exception? e = null)
    {
        success = false;
        exception = e;
        ecsEvent = @event;
    }

    public void Dispose()
    {
        ecsEvent.EventDuration = sw.Elapsed;
        ecsEvent.EventOutcome = Success;
        ecsEvent.EventMessage = Message;

        if (success)
        {
            switch (levelSuccess)
            {
                case LogLevel.Trace:
                    logger.LogTraceEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Debug:
                    logger.LogDebugEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Information:
                    logger.LogInformationEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Warning:
                    logger.LogWarningEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Error:
                    logger.LogErrorEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Critical:
                    logger.LogCriticalEvent(ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.None:
                default:   
                    break;
            }
        }
        else
        {
            switch (levelFailure)
            {
                case LogLevel.Trace:
                    logger.LogTraceEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Debug:
                    logger.LogDebugEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Information:
                    logger.LogInformationEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Warning:
                    logger.LogWarningEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Error:
                    logger.LogErrorEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.Critical:
                    logger.LogCriticalEvent(exception, ecsEvent, eventStart, memberName, sourceFilePath, sourceLineNumber);
                    break;
                case LogLevel.None:
                default:   
                    break;
            }
        }
    }
}