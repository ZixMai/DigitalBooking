using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Templates.Themes;
using SerilogTracing.Expressions;

namespace DigitalBooking.Presentation.Logger;

public static class LoggerInjection
{
    private static LoggerConfiguration EnrichLoggerConfiguration(LoggerConfiguration loggerConfiguration) =>
        loggerConfiguration.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithClientIp()
            .Enrich.WithCorrelationId()
            .Enrich.WithRequestHeader("x-trace-id")
            .Enrich.WithExceptionDetails(
                new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers([new DbUpdateExceptionDestructurer()])
            )
            .WriteTo.Console(Formatters.CreateConsoleTextFormatter(
                TemplateTheme.Code));

    public static void ConfigureLogger(this IServiceCollection services)
    {
        services.AddHttpLogging
        (options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
                options.ResponseBodyLogLimit = 12 * 1024;
            }
        );

        Log.Logger = EnrichLoggerConfiguration(new LoggerConfiguration()).CreateBootstrapLogger();
        services.AddSerilog((serviceProvider, lc) =>
            EnrichLoggerConfiguration(lc).ReadFrom.Services(serviceProvider)
        );
    }
}