using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class TeacherGroup : Group
{
    public TeacherGroup()
    {
        Configure(RouteGroups.Teacher.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Teacher));
        });
    }
}
