using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Teacher;

public sealed class ReadTeachersEndpoint(
    ITeacherRepository studentRepository,
    IUserRepository userRepository
) : Endpoint<ReadTeacherRequest, ReadTeacherResponse>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<TeacherGroup>();

        Description(b => b
            .WithName("ReadTeachers")
            .WithTags(RouteGroups.Teacher)
            .Produces<ReadTeacherResponse>());
    }

    public override async Task HandleAsync(ReadTeacherRequest req, CancellationToken ct)
    {
        var user = await userRepository.GetUserAsync(req.Id, ct);
        if (user == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        var profile = await studentRepository.GetAsync(req.Id, ct);
        if (profile == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        await Send.OkAsync(new ReadTeacherResponse
        (
            Id: profile.UserId,
            Username: profile.User.Username,
            Fullname: profile.User.Fullname,
            ScienceCloudLink: profile.ScienceCloudLink,
            DepartmentId: profile.User.DepartmentId,
            DepartmentName: profile.User.Department.Title,
            CreatedAt: profile.CreatedAt,
            UpdatedAt: profile.User.UpdatedAt
        ), ct);
    }
}

public sealed record ReadTeacherRequest([FromRoute] Guid Id);
public sealed record ReadTeacherResponse(
    Guid Id,
    string Username,
    string Fullname,
    string? ScienceCloudLink,
    long DepartmentId,
    string DepartmentName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);