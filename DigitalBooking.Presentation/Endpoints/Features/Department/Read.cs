using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Department;

public sealed class ReadDepartmentEndpoint(IDepartmentRepository departmentRepository) : Endpoint<ReadDepartmentRequest, Domain.Department>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<DepartmentGroup>();

        Description(b => b
            .WithName("ReadDepartment")
            .WithTags(RouteGroups.Department)
            .Produces<Domain.Department>());
    }

    public override async Task HandleAsync(ReadDepartmentRequest req, CancellationToken ct)
    {
        var result = await departmentRepository.GetAsync(req.Id, ct);
        if (result == null) { await Send.NotFoundAsync(ct); return; }
        await Send.OkAsync(result, ct);
    }
}

public sealed record ReadDepartmentRequest([FromRoute] long Id);
