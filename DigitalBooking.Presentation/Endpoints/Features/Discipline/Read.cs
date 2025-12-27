using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Discipline;

public sealed class ReadDisciplineEndpoint(IDisciplineRepository disciplineRepository) : Endpoint<ReadDisciplineRequest, Domain.Discipline>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<DisciplineGroup>();

        Description(b => b
            .WithName("ReadDiscipline")
            .WithTags(RouteGroups.Discipline)
            .Produces<Domain.Discipline>());
    }

    public override async Task HandleAsync(ReadDisciplineRequest req, CancellationToken ct)
    {
        var result = await disciplineRepository.GetAsync(req.Id, ct);
        if (result == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(result, ct);
    }
}

public sealed record ReadDisciplineRequest([FromRoute] long Id);
