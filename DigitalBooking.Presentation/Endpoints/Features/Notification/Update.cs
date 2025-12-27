using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Notification;

public sealed class UpdateNotificationEndpoint(INotificationRepository notificationRepository) : Endpoint<UpdateNotificationRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<NotificationGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("UpdateNotification")
            .WithTags(RouteGroups.Notification));
    }

    
    public override async Task HandleAsync(UpdateNotificationRequest req, CancellationToken ct)
    {
        if (await notificationRepository.GetAsync(req.Id, ct) is null)
        { 
            await Send.NotFoundAsync(ct);
            return;
        }
        await notificationRepository.UpdateAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateNotificationRequest(long Id);