using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Department;

public sealed class CreateDepartmentEndpoint(IDepartmentRepository departmentRepository) : Endpoint<CreateDepartmentRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<DepartmentGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("CreateDepartment")
            .WithTags(RouteGroups.Department));
    }

    internal sealed class Validator : Validator<CreateDepartmentRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(100).WithMessage("Превышена максимальная длина названия");
        }
    }
    
    public override async Task HandleAsync(CreateDepartmentRequest req, CancellationToken ct)
    {
        var entity = await departmentRepository.GetByNameAsync(req.Title, ct);
        if (entity is not null) {
            AddError("Already exists.");
            await Send.ErrorsAsync(cancellation: ct); 
            return;
        }
        await departmentRepository.CreateAsync(new Domain.Department{Title = req.Title}, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateDepartmentRequest(string Title);
