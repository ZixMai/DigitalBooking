using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Department;

public sealed class DeleteDepartmentEndpoint(IDepartmentRepository departmentRepository) : Endpoint<DeleteDepartmentRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<DepartmentGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("DeleteDepartment")
            .WithTags(RouteGroups.Department));
    }

    internal sealed class Validator : Validator<DeleteDepartmentRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEqual(1).WithMessage("Невозможно удалить");
        }
    }
    
    public override async Task HandleAsync(DeleteDepartmentRequest req, CancellationToken ct)
    {
        var entity = await departmentRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            AddError("Not found.");
            await Send.NotFoundAsync(ct); 
            return;
        }
        await departmentRepository.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteDepartmentRequest([FromRoute] long Id);
