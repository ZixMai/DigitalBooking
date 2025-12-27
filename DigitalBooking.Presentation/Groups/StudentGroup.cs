using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class StudentGroup : Group
{
    public StudentGroup()
    {
        Configure(RouteGroups.Student.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Student));
        });
    }
}
