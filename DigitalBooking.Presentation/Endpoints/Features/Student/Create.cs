using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Student;

public sealed class CreateStudentEndpoint(
    IStudentRepository studentRepository,
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository
) : Endpoint<CreateStudentRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<StudentGroup>();
        Roles(nameof(Role.STUDENT));
        Description(b => b
            .WithName("CreateStudent")
            .WithTags(RouteGroups.Student));
    }

    internal sealed class Validator : Validator<CreateStudentRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ник должен быть не пустой.")
                .MinimumLength(8).WithMessage("Ник должен быть не короче 8 символов")
                .MaximumLength(200).WithMessage("Превышена максимальная длина ника");
            
            
            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("ФИО должно быть не пустым.")
                .MinimumLength(8).WithMessage("ФИО должно быть не короче 8 символов")
                .MaximumLength(200).WithMessage("Превышена максимальная длина ФИО");
            
            RuleFor(x => x.ContactLink)
                .NotEmpty().WithMessage("Тэг должен быть не пустым.")
                .MinimumLength(5).WithMessage("Тэг должен быть не короче 8 символов")
                .MaximumLength(200).WithMessage("Превышена максимальная длина тэга");
        }
    }

    public override async Task HandleAsync(CreateStudentRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        
        var group = await userRepository.GetGroupAsync(req.GroupId, ct);
        if (group is null) { AddError("Группа не найдена"); }
        var department = await departmentRepository.GetAsync(req.DepartmentId, ct);
        if (department is null) { AddError("Отделение не найдено"); }

        if (ValidationFailed)
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }
        
        var user = await userRepository.GetUserAsync(userId, ct);
        user!.Username = req.Username;
        user.Fullname = req.Fullname;
        user.DepartmentId = req.DepartmentId;
        user.UpdatedAt = DateTimeConverter.RemoveTimeZone(DateTime.UtcNow);
        var profile = new StudentProfile
        {
            UserId = userId,
            GroupId = req.GroupId,
            ContactLink = req.ContactLink,
        };
        
        await studentRepository.CreateAsync(user, profile, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateStudentRequest(
    string Username,
    string Fullname,
    long DepartmentId,
    long GroupId,
    string? ContactLink
);