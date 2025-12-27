using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Notification;

public sealed class GetAllNotificationEndpoint(INotificationRepository notificationRepository)
    : EndpointWithoutRequest<GetAllNotificationResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<NotificationGroup>();

        Description(b => b
            .WithName("GetAllNotification")
            .WithTags(RouteGroups.Notification)
            .Produces<GetAllNotificationResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await notificationRepository.GetAllAsync(ct);
        await Send.OkAsync(new GetAllNotificationResponse(result), ct);
    }
}

public sealed record GetAllNotificationResponse(List<Domain.Notification> Items);
