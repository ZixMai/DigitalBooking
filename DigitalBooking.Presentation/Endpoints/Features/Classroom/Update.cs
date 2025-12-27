using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Classroom;

public sealed class UpdateClassroomEndpoint(
    IDepartmentRepository departmentRepository,
    IClassroomRepository classroomRepository
) : Endpoint<UpdateClassroomRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<ClassroomGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("UpdateClassroom")
            .WithTags(RouteGroups.Classroom));
    }

    internal sealed class Validator : Validator<Domain.Classroom>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(100).WithMessage("Превышена максимальная длина названия");
        }
    }
    
    public override async Task HandleAsync(UpdateClassroomRequest req, CancellationToken ct)
    {
        var classroom = await classroomRepository.GetAsync(req.Id, ct);
        if (classroom is null)
        { 
            await Send.NotFoundAsync(ct);
            return;
        }
        var classroomType = await classroomRepository.GetTypeAsync(req.ClassroomType, ct);
        var department = await departmentRepository.GetByNameAsync(req.Department, ct);
        if (classroomType is null) { AddError("Тип аудитории не найден"); }
        if (department is null) { AddError("Отделение не найдено"); }
        if (ValidationFailed)
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }
        await classroomRepository.UpdateAsync(new Domain.Classroom
        {
            Id = req.Id,
            Title = req.Title,
            OwnerDepartmentId = department!.Id,
            BookingSlotsLimit = req.BookingSlotsLimit,
            Capacity = req.Capacity,
            ClassroomTypeId = classroomType!.Id
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateClassroomRequest(
    long Id,
    string Title,
    string Department,
    short BookingSlotsLimit,
    short? Capacity,
    string ClassroomType
);