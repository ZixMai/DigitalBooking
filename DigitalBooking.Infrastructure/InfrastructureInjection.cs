using DigitalBooking.Application.Abstractions;
using DigitalBooking.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetRequiredSection(nameof(DbConfiguration)).Get<DbConfiguration>();

        services.AddDbContext<DbContext>(options =>
        {
            options.UseNpgsql(config!.CreateConnectionString());
        });
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IClassroomRepository, ClassroomRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IDisciplineRepository, DisciplineRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<ILessonsRepository, LessonsRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        
        return services;
    }
}
