using FastEndpoints;

namespace DigitalBooking.Presentation.Groups;

public sealed class DisciplineGroup : Group
{
    public DisciplineGroup()
    {
        Configure(RouteGroups.Discipline.ToLower(), ep =>
        {
            ep.Description(x => x.WithTags(RouteGroups.Discipline));
        });
    }
}
