using System.Text.Json;
using DigitalBooking.Presentation.Middlewares;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;

namespace DigitalBooking.Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureHttpPipeline(
        this WebApplication app,
        IWebHostEnvironment env)
    {
        app.UseCors(cors => cors
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

        app.UseSerilogRequestLogging();

        app.UseMiddleware<RetryMiddleware>();

        app.UseDefaultExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api/v1";
            c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

            c.Endpoints.Configurator = ep =>
            {
                ep.Description(b => b
                    .Produces<ErrorResponse>(400, "application/problem+json")
                    .Produces<ProblemDetails>(500));
            };
        });

        app.UseSwaggerGen();

        if (env.IsDevelopment())
            app.MapOpenApi();

        app.UseHttpsRedirection();

        return app;
    }
}