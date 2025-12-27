using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class NotificationGroup : Group
{
    public NotificationGroup()
    {
        Configure(RouteGroups.Notification.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Notification));
        });
    }
}
