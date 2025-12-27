using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Library;

public sealed class GetAllLibraryPublicationEndpoint(ILibraryRepository libraryPublicationRepository)
    : EndpointWithoutRequest<GetAllLibraryPublicationResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<LibraryGroup>();

        Description(b => b
            .WithName("GetAllLibraryPublication")
            .WithTags(RouteGroups.Library)
            .Produces<GetAllLibraryPublicationResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await libraryPublicationRepository.GetAllAsync(ct);
        await Send.OkAsync(new GetAllLibraryPublicationResponse(result), ct);
    }
}

public sealed record GetAllLibraryPublicationResponse(List<Domain.LibraryPublication> Items);
