using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Lessons;

public sealed class ReadLessonsEndpoint(ILessonsRepository lessonsRepository) : Endpoint<ReadLessonsRequest, ReadLessonsResponse>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<LessonsGroup>();

        Description(b => b
            .WithName("ReadLessons")
            .WithTags(RouteGroups.Lessons)
            .Produces<ReadLessonsResponse>());
    }

    public override async Task HandleAsync(ReadLessonsRequest req, CancellationToken ct)
    {
        var lesson = await lessonsRepository.GetAsync(req.Id, ct);
        if (lesson == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(new ReadLessonsResponse
        (
            Id: lesson.Id,
            DisciplineName: lesson.Discipline.Title,
            TeacherId: lesson.TeacherId,
            TeacherFullname: lesson.Teacher.Fullname,
            GroupName: lesson.Group.GetDisplayName(),
            LmsLink: lesson.LmsLink,
            CreatedAt: lesson.CreatedAt
        ), ct);
    }
}

public sealed record ReadLessonsRequest([FromRoute] long Id);
public sealed record ReadLessonsResponse(
    long Id,
    string DisciplineName,
    Guid TeacherId,
    string TeacherFullname,
    string GroupName,
    string? LmsLink,
    DateTime CreatedAt
);
