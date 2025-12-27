using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Notification;

public sealed class DeleteNotificationEndpoint(INotificationRepository notificationRepository)
    : Endpoint<DeleteNotificationRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<NotificationGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("DeleteNotification")
            .WithTags(RouteGroups.Notification));
    }

    public override async Task HandleAsync(DeleteNotificationRequest req, CancellationToken ct)
    {
        await notificationRepository.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteNotificationRequest([FromRoute] long Id);