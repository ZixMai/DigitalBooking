using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Discipline;

public sealed class DeleteDisciplineEndpoint(IDisciplineRepository disciplineRepository) : Endpoint<DeleteDisciplineRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<DisciplineGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("DeleteDiscipline")
            .WithTags(RouteGroups.Discipline));
    }

    public override async Task HandleAsync(DeleteDisciplineRequest req, CancellationToken ct)
    {
        var entity = await disciplineRepository.GetAsync(req.Id, ct);
        if (entity is null)
        {
            AddError("Not found.");
            await Send.NotFoundAsync(ct);
            return;
        }

        await disciplineRepository.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteDisciplineRequest([FromRoute] long Id);
