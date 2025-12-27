using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Lessons;

public sealed class GetAllLessonsEndpoint(ILessonsRepository lessonsRepository)
    : EndpointWithoutRequest<GetAllLessonsResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<LessonsGroup>();

        Description(b => b
            .WithName("GetAllLessons")
            .WithTags(RouteGroups.Lessons)
            .Produces<GetAllLessonsResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var lessons = await lessonsRepository.GetAllAsync(ct);
        var items = lessons
            .Select(lesson => new ReadLessonsResponse
            (
                Id: lesson.Id,
                DisciplineName: lesson.Discipline.Title,
                TeacherId: lesson.TeacherId,
                TeacherFullname: lesson.Teacher.Fullname,
                GroupName: lesson.Group.GetDisplayName(),
                LmsLink: lesson.LmsLink,
                CreatedAt: lesson.CreatedAt
            ))
            .ToList();

        await Send.OkAsync(new GetAllLessonsResponse(items), ct);
    }
}

public sealed record GetAllLessonsResponse(List<ReadLessonsResponse> Items);
