using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Library;

public sealed class ReadLibraryPublicationEndpoint(ILibraryRepository libraryPublicationRepository)
    : Endpoint<ReadLibraryPublicationRequest>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<LibraryGroup>();

        Description(b => b
            .WithName("ReadLibraryPublication")
            .WithTags(RouteGroups.Library));
    }

    public override async Task HandleAsync(ReadLibraryPublicationRequest req, CancellationToken ct)
    {
        var result = await libraryPublicationRepository.GetAsync(req.Id, ct);
        if (result == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        var stream = Stream.Null; // s3 заглушка

        await Send.StreamAsync(
            stream: stream,
            fileName: result.Title,
            fileLengthBytes: 0,
            contentType: "application/octet-stream",
            enableRangeProcessing: false,
            cancellation: ct);
    }
}

public sealed record ReadLibraryPublicationRequest([FromRoute] Guid Id);