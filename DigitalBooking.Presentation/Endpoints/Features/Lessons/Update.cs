using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Lessons;

public sealed class UpdateLessonEndpoint(
    ILessonsRepository lessonRepository
) : Endpoint<UpdateLessonRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<LessonsGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("UpdateLesson")
            .WithTags(RouteGroups.Lessons));
    }

    internal sealed class Validator : Validator<UpdateLessonRequest>
    {
        public Validator()
        {
            RuleFor(x => x.LmsLink)
                .NotEmpty().WithMessage("Ссылка должна быть не пустой.")
                .MaximumLength(200).WithMessage("Превышена максимальная длина ссылки");
        }
    }
    
    public override async Task HandleAsync(UpdateLessonRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        var entity = await lessonRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            await Send.NotFoundAsync(ct); 
            return;
        }
        if (entity.TeacherId != userId && role != Role.ADMIN)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        
        await lessonRepository.UpdateAsync(req.Id, req.LmsLink, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateLessonRequest(
    long Id,
    string LmsLink
);