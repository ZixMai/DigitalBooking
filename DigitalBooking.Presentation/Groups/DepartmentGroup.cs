using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class DepartmentGroup : Group
{
    public DepartmentGroup()
    {
        Configure(RouteGroups.Department.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Department));
        });
    }
}
