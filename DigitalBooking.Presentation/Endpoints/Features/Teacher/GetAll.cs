using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Teacher;

public sealed class GetAllTeacherEndpoint(ITeacherRepository teacherRepository)
    : EndpointWithoutRequest<GetAllTeacherResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<TeacherGroup>();

        Description(b => b
            .WithName("GetAllTeacher")
            .WithTags(RouteGroups.Teacher)
            .Produces<GetAllTeacherResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await teacherRepository.GetAllAsync(ct);
        var items = result
            .Select(profile => new ReadTeacherResponse
            (
                Id: profile.UserId,
                Username: profile.User.Username,
                Fullname: profile.User.Fullname,
                ScienceCloudLink: profile.ScienceCloudLink,
                DepartmentId: profile.User.DepartmentId,
                DepartmentName: profile.User.Department.Title,
                CreatedAt: profile.CreatedAt,
                UpdatedAt: profile.User.UpdatedAt
            ))
            .ToList();

        await Send.OkAsync(new GetAllTeacherResponse(items), ct);
    }
}

public sealed record GetAllTeacherResponse(List<ReadTeacherResponse> Items);
