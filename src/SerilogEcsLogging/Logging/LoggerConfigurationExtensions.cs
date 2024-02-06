using System.Reflection;
using Elastic.CommonSchema.Serilog;
using Serilog;
using Serilog.Events;

namespace SerilogEcsLogging.Logging;

public static class LoggerConfigurationExtensions
{
    public const string TraceTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}][{MachineName}][{Level:u3}][{SourceContext}][{ThreadId}]{Scope} {Message}{NewLine}{Exception}";

    public static EcsTextFormatter EcsTextFormatter => new EcsTextFormatter(new EcsTextFormatterConfiguration { 
        IncludeHost = true, 
        IncludeProcess = true, 
        IncludeUser = true, 
        MapCustom = EcsMapper.MapLogEvent, 
        LogEventPropertiesToFilter = new HashSet<string> {"metadata.*", "labels.*"} 
    });
    
    public static LoggerConfiguration ConfigureEcs(this LoggerConfiguration configuration, bool logEcsEvents = true, bool logToConsole = true, bool consoleToStdErr = false, string? logFilePath = null, string? httpEndpoint = null, TimeSpan? httpPostPeriod = null, int? logEventsInHttpBatchLimit = 1000)
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
                    c.Console(EcsTextFormatter, standardErrorFromLevel: stdErrFromLevel);
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
                logFilePath = Path.Combine(directory, $"{filename}.ECS{extension}");
                    
                configuration.WriteTo.Async(c => c.File(EcsTextFormatter, logFilePath, rollingInterval: RollingInterval.Day));
            }
            else
            {
                configuration.WriteTo.Async(c => c.File(logFilePath, rollingInterval: RollingInterval.Day, outputTemplate: TraceTemplate));
            }
        }

        if (!string.IsNullOrEmpty(httpEndpoint))
        {
            var path = logFilePath;
            if (string.IsNullOrEmpty(path))
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
                path = Path.Combine(HostBuilderExtensions.DefaultLogDirectory, assembly.GetName().Name ?? Guid.NewGuid().ToString("N"));
            }
            
            configuration.WriteTo.DurableHttpUsingFileSizeRolledBuffers(httpEndpoint, $"{path}-buffer", textFormatter: logEcsEvents ? EcsTextFormatter : null, period: httpPostPeriod, logEventsInBatchLimit: logEventsInHttpBatchLimit);
        }

        return configuration;
    }
}