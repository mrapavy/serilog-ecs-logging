using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace SerilogEcsLogging.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSerilogEvents(this IHostBuilder builder, Action<HostBuilderContext, LoggerConfiguration>? configureLogger = null, bool logEcsEvents = true, bool logToConsole = true, bool consoleToStdErr = false, string? logFilePath = null)
    {
        return builder.UseSerilog((context, configuration) => {
            configuration.ConfigureEcs(logEcsEvents, logToConsole, consoleToStdErr, logFilePath, context);
            configuration.ReadFrom.Configuration(context.Configuration);
            configureLogger?.Invoke(context, configuration);
        });
    }
}