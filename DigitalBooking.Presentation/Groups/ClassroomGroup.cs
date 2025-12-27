using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class ClassroomGroup : Group
{
    public ClassroomGroup()
    {
        Configure(RouteGroups.Classroom.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Classroom));
        });
    }
}
