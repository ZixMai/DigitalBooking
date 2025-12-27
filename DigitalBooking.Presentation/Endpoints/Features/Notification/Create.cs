using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Notification;

public sealed class CreateNotificationEndpoint(INotificationRepository notificationRepository) : Endpoint<CreateNotificationRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<NotificationGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("CreateNotification")
            .WithTags(RouteGroups.Notification));
    }
    
    public override async Task HandleAsync(CreateNotificationRequest req, CancellationToken ct)
    {
        await notificationRepository.CreateAsync(new Domain.Notification
        {
            ReceiverId = null,
            Message = req.Message,
            BookingId = null,
            Read = false,
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateNotificationRequest(string Message);