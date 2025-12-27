using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Teacher;

public sealed class UpdateTeacherEndpoint(
    ITeacherRepository studentRepository,
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository
) : Endpoint<UpdateTeacherRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<TeacherGroup>();
        Roles(nameof(Role.TEACHER), nameof(Role.ADMIN));
        Description(b => b
            .WithName("UpdateTeacher")
            .WithTags(RouteGroups.Teacher));
    }

    internal sealed class Validator : Validator<UpdateTeacherRequest>
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
            
            RuleFor(x => x.ScienceCloudLink)
                .NotEmpty().WithMessage("Ссылка должна быть не пустой.")
                .MaximumLength(1000).WithMessage("Превышена максимальная длина тэга");
        }
    }
    
    public override async Task HandleAsync(UpdateTeacherRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        var department = await departmentRepository.GetAsync(req.DepartmentId, ct);
        if (department is null) { AddError("Отделение не найдено"); }
        var profile = await studentRepository.GetAsync(userId, ct);
        if (profile is null) { AddError("Профиль не найден"); }

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
        
        profile!.AcademicDegree = req.AcademicDegree;
        profile.ScienceCloudLink = req.ScienceCloudLink;
        await studentRepository.UpdateAsync(user, profile, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateTeacherRequest(
    string Username,
    string Fullname,
    long DepartmentId,
    string AcademicDegree,
    string ScienceCloudLink
);