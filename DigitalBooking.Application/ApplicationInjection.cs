using DigitalBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalBooking.Application;

public static class ApplicationInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddSingleton<HashService>();
        services.AddSingleton<JwtService>();
        
        return services;
    }
}