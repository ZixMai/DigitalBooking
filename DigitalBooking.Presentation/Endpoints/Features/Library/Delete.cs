using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Library;

public sealed class DeleteLibraryPublicationEndpoint(ILibraryRepository libraryPublicationRepository)
    : Endpoint<DeleteLibraryPublicationRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<LibraryGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("DeleteLibraryPublication")
            .WithTags(RouteGroups.Library));
    }

    public override async Task HandleAsync(DeleteLibraryPublicationRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        var entity = await libraryPublicationRepository.GetAsync(req.Id, ct);
        if (entity is null)
        {
            AddError("Not found.");
            await Send.NotFoundAsync(ct);
            return;
        }

        if (entity.CreatorId != userId)
        {
            AddError("Вы не можете редактировать чужие публикации.");
            await Send.ForbiddenAsync(ct);
            return;
        }

        await libraryPublicationRepository.DeleteAsync(req.Id, ct);
        // todo: s3
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteLibraryPublicationRequest([FromRoute] Guid Id);