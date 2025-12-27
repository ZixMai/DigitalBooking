using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class BookingGroup : Group
{
    public BookingGroup()
    {
        Configure(RouteGroups.Booking.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Booking));
        });
    }
}
