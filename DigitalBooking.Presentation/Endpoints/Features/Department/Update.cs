using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Department;

public sealed class UpdateDepartmentEndpoint(IDepartmentRepository departmentRepository) : Endpoint<Domain.Department>
{
    public override void Configure()
    {
        Put("/");
        Group<DepartmentGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("UpdateDepartment")
            .WithTags(RouteGroups.Department));
    }

    internal sealed class Validator : Validator<Domain.Department>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(100).WithMessage("Превышена максимальная длина названия");
        }
    }
    
    public override async Task HandleAsync(Domain.Department reqEntity, CancellationToken ct)
    {
        if (await departmentRepository.GetAsync(reqEntity.Id, ct) is null)
        { 
            await Send.NotFoundAsync(ct);
            return;
        }
        await departmentRepository.UpdateAsync(reqEntity, ct);
        await Send.NoContentAsync(ct);
    }
}

