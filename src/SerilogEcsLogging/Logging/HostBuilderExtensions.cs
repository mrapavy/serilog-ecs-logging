﻿using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SerilogEcsLogging.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSerilogEvents(this IHostBuilder builder, Action<HostBuilderContext, LoggerConfiguration>? configureLogger = null, bool logEcsEvents = true, bool logToConsole = true)
    {
        return builder.UseSerilog((context, configuration) => {
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
                configuration.WriteTo.Async(c => {
                    if (logEcsEvents)
                    {
                        c.Console(new EcsTextFormatter(new EcsTextFormatterConfiguration().MapCustom(EcsMapper.MapLogEvent).MapExceptions(true).MapCurrentThread(true)
                            .MapHttpContext(context.Configuration.Get<HttpContextAccessor>())));
                    }
                    else
                    {
                        c.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}][{Level:u3}][{SourceContext}][{ThreadId}]{Scope} {Message}{NewLine}{Exception}");
                    }
                });
            }

            configuration.ReadFrom.Configuration(context.Configuration);
            configureLogger?.Invoke(context, configuration);
        });
    }
}