using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class LibraryGroup : Group
{
    public LibraryGroup()
    {
        Configure(RouteGroups.Library.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Library));
        });
    }
}
