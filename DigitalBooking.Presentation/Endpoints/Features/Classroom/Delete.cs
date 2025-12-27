using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Classroom;

public sealed class DeleteClassroomEndpoint(IClassroomRepository classroomRepository) : Endpoint<DeleteClassroomRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<ClassroomGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("DeleteClassroom")
            .WithTags(RouteGroups.Classroom));
    }

    public override async Task HandleAsync(DeleteClassroomRequest req, CancellationToken ct)
    {
        var entity = await classroomRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            AddError("Not found.");
            await Send.NotFoundAsync(ct); 
            return;
        }
        await classroomRepository.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteClassroomRequest([FromRoute] long Id);