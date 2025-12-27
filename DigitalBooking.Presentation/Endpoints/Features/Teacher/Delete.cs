using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Teacher;

public sealed class DeleteTeacherEndpoint(IUserRepository userRepository) : Endpoint<DeleteTeacherRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<TeacherGroup>();
        Description(b => b
            .WithName("DeleteTeacher")
            .WithTags(RouteGroups.Teacher));
    }

    public override async Task HandleAsync(DeleteTeacherRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        if ((role != Role.ADMIN && userId != req.Id) || role == Role.STUDENT)
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

public sealed record DeleteTeacherRequest([FromRoute] Guid Id);