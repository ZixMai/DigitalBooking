using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Teacher;

public sealed class CreateTeacherEndpoint(
    ITeacherRepository teacherRepository,
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository
) : Endpoint<CreateTeacherRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<TeacherGroup>();
        Description(b => b
            .WithName("CreateTeacher")
            .WithTags(RouteGroups.Teacher));
    }

    internal sealed class Validator : Validator<CreateTeacherRequest>
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

    public override async Task HandleAsync(CreateTeacherRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        if (role == Role.STUDENT)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        
        var department = await departmentRepository.GetAsync(req.DepartmentId, ct);
        if (department is null)
        {
            AddError("Отделение не найдено");
            await Send.NotFoundAsync(ct);
            return;
        }
        
        var user = await userRepository.GetUserAsync(userId, ct);
        user!.Username = req.Username;
        user.Fullname = req.Fullname;
        user.DepartmentId = req.DepartmentId;
        user.UpdatedAt = DateTimeConverter.RemoveTimeZone(DateTime.UtcNow);
        var profile = new TeacherProfile
        {
            UserId = userId,
            AcademicDegree = req.AcademicDegree,
            ScienceCloudLink = req.ScienceCloudLink,
        };
        
        await teacherRepository.CreateAsync(user, profile, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateTeacherRequest(
    string Username,
    string Fullname,
    long DepartmentId,
    string AcademicDegree,
    string ScienceCloudLink
);