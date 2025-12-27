using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Classroom;

public sealed class CreateClassroomEndpoint(
    IUserRepository userRepository,
    IClassroomRepository classroomRepository
) : Endpoint<CreateClassroomRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<ClassroomGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("CreateClassroom")
            .WithTags(RouteGroups.Classroom));
    }

    internal sealed class Validator : Validator<CreateClassroomRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(100).WithMessage("Превышена максимальная длина названия");
            RuleFor(x => x.BookingSlotsLimit)
                .LessThan((short)10).When(x => x.BookingSlotsLimit != null).WithMessage("Слишком много слотов");
        }
    }

    public override async Task HandleAsync(CreateClassroomRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();

        var classroomType = await classroomRepository.GetTypeAsync(req.ClassroomTypeId, ct);
        if (classroomType == null)
        {
            AddError("Не найден тип аудитории");
            await Send.NotFoundAsync(ct);
            return;
        }

        var entity = await classroomRepository.GetByNameAsync(req.Title, ct);
        if (entity is not null)
        {
            AddError("Already exists.");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var user = await userRepository.GetUserAsync(userId, ct);
        await classroomRepository.CreateAsync(new Domain.Classroom
        {
            Title = req.Title,
            OwnerDepartmentId = user!.DepartmentId,
            BookingSlotsLimit = req.BookingSlotsLimit ?? 1,
            Capacity = req.Capacity
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateClassroomRequest(
    string Title,
    short? BookingSlotsLimit,
    short? Capacity,
    long ClassroomTypeId
);