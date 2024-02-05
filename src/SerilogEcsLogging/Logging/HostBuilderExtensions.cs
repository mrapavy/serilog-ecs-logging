using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SerilogEcsLogging.Logging;

public static class HostBuilderExtensions
{
    public static string DefaultLogDirectory => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create)}/EFTLog" : "/var/log";
    
    public static bool RunningInContainer => string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.InvariantCultureIgnoreCase);
    
    public static IHostBuilder UseSerilogEvents(this IHostBuilder builder, Action<HostBuilderContext, LoggerConfiguration>? configureLogger = null, bool logEcsEvents = true, bool logToConsole = true, bool consoleToStdErr = false, string? logFilePath = null, string? httpEndpoint = null, TimeSpan? httpPostPeriod = null, int? logEventsInHttpBatchLimit = 1000)
    {
        return builder.UseSerilog((context, configuration) => {
            configuration.ConfigureEcs(logEcsEvents, logToConsole, consoleToStdErr, logFilePath, httpEndpoint, httpPostPeriod, logEventsInHttpBatchLimit);
            configuration.ReadFrom.Configuration(context.Configuration);
            configureLogger?.Invoke(context, configuration);
        });
    }
}