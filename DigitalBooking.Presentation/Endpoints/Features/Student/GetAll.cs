using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Student;

public sealed class GetAllStudentEndpoint(IStudentRepository studentRepository)
    : EndpointWithoutRequest<GetAllStudentResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<StudentGroup>();

        Description(b => b
            .WithName("GetAllStudent")
            .WithTags(RouteGroups.Student)
            .Produces<GetAllStudentResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await studentRepository.GetAllAsync(ct);
        var items = result
            .Select(profile => new ReadStudentResponse
            (
                Id: profile.UserId,
                Username: profile.User.Username,
                Fullname: profile.User.Fullname,
                ContactLink: profile.ContactLink,
                DepartmentId: profile.User.DepartmentId,
                DepartmentName: profile.User.Department.Title,
                profile.Group.GetDisplayName(),
                CreatedAt: profile.CreatedAt,
                UpdatedAt: profile.User.UpdatedAt
            ))
            .ToList();

        await Send.OkAsync(new GetAllStudentResponse(items), ct);
    }
}

public sealed record GetAllStudentResponse(List<ReadStudentResponse> Items);
