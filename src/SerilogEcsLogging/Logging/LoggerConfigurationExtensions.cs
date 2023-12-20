using Elastic.CommonSchema.Serilog;
using Serilog;
using Serilog.Events;

namespace SerilogEcsLogging.Logging;

public static class LoggerConfigurationExtensions
{
    public const string TraceTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}][{MachineName}][{Level:u3}][{SourceContext}][{ThreadId}]{Scope} {Message}{NewLine}{Exception}";

    public static EcsTextFormatter CreateEcsTextFormatter() => new EcsTextFormatter(new EcsTextFormatterConfiguration { 
        IncludeHost = true, 
        IncludeProcess = true, 
        IncludeUser = true, 
        MapCustom = EcsMapper.MapLogEvent, 
        LogEventPropertiesToFilter = new HashSet<string> {"metadata.*"} 
    });
    
    public static LoggerConfiguration ConfigureEcs(this LoggerConfiguration configuration, bool logEcsEvents = true, bool logToConsole = true, bool consoleToStdErr = false, string? logFilePath = null)
    {
        configuration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationId()
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion();

        if (logToConsole)
        {
            LogEventLevel? stdErrFromLevel = consoleToStdErr ? LogEventLevel.Verbose : null;
            configuration.WriteTo.Async(c => {
                if (logEcsEvents)
                {
                    c.Console(CreateEcsTextFormatter(), standardErrorFromLevel: stdErrFromLevel);
                }
                else
                {
                    c.Console(outputTemplate: TraceTemplate, standardErrorFromLevel: stdErrFromLevel);
                }
            });
        }

        if (logFilePath != null)
        {
            if (logEcsEvents)
            {
                var directory = Path.GetDirectoryName(logFilePath) ?? string.Empty;
                var filename = Path.GetFileNameWithoutExtension(logFilePath);
                var extension = Path.GetExtension(logFilePath);
                logFilePath = Path.Combine(directory, $"{filename}.ECS.{extension}");
                    
                configuration.WriteTo.Async(c => c.File(CreateEcsTextFormatter(), logFilePath, rollingInterval: RollingInterval.Day));
            }
            else
            {
                configuration.WriteTo.Async(c => c.File(logFilePath, rollingInterval: RollingInterval.Day, outputTemplate: TraceTemplate));
            }
        }

        return configuration;
    }
}