using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class LessonsGroup : Group
{
    public LessonsGroup()
    {
        Configure(RouteGroups.Lessons.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Lessons));
        });
    }
}
