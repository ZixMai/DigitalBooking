using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Department;

public sealed class GetAllDepartmentEndpoint(IDepartmentRepository departmentRepository)
    : EndpointWithoutRequest<GetAllDepartmentResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<DepartmentGroup>();

        Description(b => b
            .WithName("GetAllDepartment")
            .WithTags(RouteGroups.Department)
            .Produces<GetAllDepartmentResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await departmentRepository.GetAllAsync(ct);
        await Send.OkAsync(new GetAllDepartmentResponse(result), ct);
    }
}

public sealed record GetAllDepartmentResponse(List<Domain.Department> Items);
