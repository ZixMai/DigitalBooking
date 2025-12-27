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
        services.ConfigureLogger();
        services.RegisterApplication();
        services.AddTransient<RetryMiddleware>();

        services.RegisterInfrastructure(configuration);

        services.AddCors();

        services
            .AddAuthenticationJwtBearer(
                s => s.SigningKey = configuration["Jwt:SigningKey"]!, 
                bearer =>
                {
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"]!,

                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"]!,

                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
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