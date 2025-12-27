using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Lessons;

public sealed class DeleteLessonEndpoint(ILessonsRepository lessonsRepository) : Endpoint<DeleteLessonRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<LessonsGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("DeleteLesson")
            .WithTags(RouteGroups.Lessons));
    }

    public override async Task HandleAsync(DeleteLessonRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        var entity = await lessonsRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            await Send.NotFoundAsync(ct); 
            return;
        }
        if (entity.TeacherId != userId && role != Role.ADMIN)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        
        await lessonsRepository.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteLessonRequest([FromRoute] long Id);