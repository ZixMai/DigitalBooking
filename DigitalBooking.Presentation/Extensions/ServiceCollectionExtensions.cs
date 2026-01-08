using System.Security.Claims;
using DigitalBooking.Application;
using DigitalBooking.Application.Services;
using DigitalBooking.Infrastructure;
using DigitalBooking.Presentation.Logger;
using DigitalBooking.Presentation.Middlewares;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.IdentityModel.Tokens;

namespace DigitalBooking.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureBootstrapLogger();
        services.RegisterApplication();
        services.AddTransient<RetryMiddleware>();
        services.AddSingleton<ResourceDetector>();
        services.ConfigureSerilog(configuration);
        services.ConfigureOTel(configuration);

        services.RegisterInfrastructure(configuration);

        services.AddCors();

        services.AddAuthenticationJwtBearer(
                s => s.SigningKey = configuration["Jwt:SigningKey"]!,
                bearer =>
                {
                    bearer.TokenValidationParameters.ValidateIssuer = true;
                    bearer.TokenValidationParameters.ValidIssuer = configuration["Jwt:Issuer"]!;

                    bearer.TokenValidationParameters.ValidateAudience = true;
                    bearer.TokenValidationParameters.ValidAudience = configuration["Jwt:Audience"]!;

                    bearer.TokenValidationParameters.ValidateLifetime = true;
                    bearer.TokenValidationParameters.ValidateIssuerSigningKey = true;

                    bearer.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                })
            .AddAuthorizationBuilder()
            .AddPolicy(ApiPolicies.IsTokenRefresh, policy =>
                policy.RequireClaim("TokenType", "refresh"))
            .AddPolicy(ApiPolicies.IsTokenAccess, policy =>
                policy.RequireClaim("TokenType", "access"));

        services.AddFastEndpoints();

        services.SwaggerDocument();

        services.Configure<JwtCreationOptions>(o =>
        {
            o.SigningKey = configuration["Jwt:SigningKey"]!;
            o.Audience = configuration["Jwt:Audience"]!;
            o.Issuer = configuration["Jwt:Issuer"]!;
        });

        return services;
    }
}