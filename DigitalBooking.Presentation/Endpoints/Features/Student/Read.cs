using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Student;

public sealed class ReadStudentsEndpoint(
    IStudentRepository studentRepository,
    IUserRepository userRepository
) : Endpoint<ReadStudentRequest, ReadStudentResponse>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<StudentGroup>();

        Description(b => b
            .WithName("ReadStudents")
            .WithTags(RouteGroups.Student)
            .Produces<ReadStudentResponse>());
    }

    public override async Task HandleAsync(ReadStudentRequest req, CancellationToken ct)
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
        
        await Send.OkAsync(new ReadStudentResponse
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
        ), ct);
    }
}

public sealed record ReadStudentRequest([FromRoute] Guid Id);
public sealed record ReadStudentResponse(
    Guid Id,
    string Username,
    string Fullname,
    string? ContactLink,
    long DepartmentId,
    string DepartmentName,
    string GroupName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);