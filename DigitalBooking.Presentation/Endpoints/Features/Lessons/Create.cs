using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Lessons;

public sealed class CreateLessonEndpoint(
    ILessonsRepository lessonRepository,
    IUserRepository userRepository,
    IDisciplineRepository disciplineRepository
) : Endpoint<CreateLessonRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<LessonsGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("CreateLesson")
            .WithTags(RouteGroups.Lessons));
    }

    internal sealed class Validator : Validator<CreateLessonRequest>
    {
        public Validator()
        {
            RuleFor(x => x.LmsLink)
                .NotEmpty().When(x => x.LmsLink != null).WithMessage("Ссылка должна быть не пустой.")
                .MaximumLength(200).When(x => x.LmsLink != null)
                .WithMessage("Превышена максимальная длина ссылки");
        }
    }

    public override async Task HandleAsync(CreateLessonRequest req, CancellationToken ct)
    {
        var user = await userRepository.GetUserAsync(req.TeacherId, ct);
        if (user is null) { AddError("Преподаватель не найден"); }

        if (user!.Role == nameof(Role.STUDENT))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        if (await disciplineRepository.GetAsync(req.DisciplineId, ct) is null) { AddError("Дисциплина не найдена"); }
        if (await userRepository.GetGroupAsync(req.GroupId, ct) is null) { AddError("Группа не найдена"); }

        if (ValidationFailed)
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }
        
        await lessonRepository.CreateAsync(new Domain.Lesson
        {
            DisciplineId = req.DisciplineId,
            TeacherId = req.TeacherId,
            GroupId = req.GroupId,
            LmsLink = req.LmsLink,
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateLessonRequest(
    long DisciplineId,
    Guid TeacherId,
    long GroupId,
    string? LmsLink
);