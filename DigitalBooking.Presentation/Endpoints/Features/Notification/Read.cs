using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Notification;

public sealed class GetNotificationEndpoint(INotificationRepository notificationRepository) : Endpoint<GetNotificationRequest, Domain.Notification>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<NotificationGroup>();

        Description(b => b
            .WithName("GetNotification")
            .WithTags(RouteGroups.Notification)
            .Produces<Domain.Notification>());
    }

    public override async Task HandleAsync(GetNotificationRequest req, CancellationToken ct)
    {
        var result = await notificationRepository.GetAsync(req.Id, ct);
        if (result == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(result, ct);
    }
}

public sealed record GetNotificationRequest([FromRoute] long Id);