using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Student;

public sealed class DeleteStudentEndpoint(IUserRepository userRepository) : Endpoint<DeleteStudentRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<StudentGroup>();
        Description(b => b
            .WithName("DeleteStudent")
            .WithTags(RouteGroups.Student));
    }

    public override async Task HandleAsync(DeleteStudentRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        if (role != Role.ADMIN && role != Role.TEACHER && userId != req.Id)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        var user = await userRepository.GetUserAsync(req.Id, ct);
        if (user == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await userRepository.SoftDeleteUserAsync(user.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteStudentRequest([FromRoute] Guid Id);