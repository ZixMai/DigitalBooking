using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Discipline;

public sealed class GetAllDisciplineEndpoint(IDisciplineRepository disciplineRepository)
    : EndpointWithoutRequest<GetAllDisciplineResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<DisciplineGroup>();

        Description(b => b
            .WithName("GetAllDiscipline")
            .WithTags(RouteGroups.Discipline)
            .Produces<GetAllDisciplineResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await disciplineRepository.GetAllAsync(ct);
        await Send.OkAsync(new GetAllDisciplineResponse(result), ct);
    }
}

public sealed record GetAllDisciplineResponse(List<Domain.Discipline> Items);
