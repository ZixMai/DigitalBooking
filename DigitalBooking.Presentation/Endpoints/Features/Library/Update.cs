using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Library;

public sealed class UpdateLibraryPublicationEndpoint(ILibraryRepository libraryPublicationRepository)
    : Endpoint<UpdateLibraryPublicationRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<LibraryGroup>();
        Description(b => b
            .WithName("UpdateLibraryPublication")
            .WithTags(RouteGroups.Library));
        Roles(nameof(Role.ADMIN));
    }

    internal sealed class Validator : Validator<UpdateLibraryPublicationRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(1000).WithMessage("Превышена максимальная длина названия");
        }
    }

    public override async Task HandleAsync(UpdateLibraryPublicationRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        var publication = await libraryPublicationRepository.GetAsync(req.Id, ct);
        if (publication is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (publication.CreatorId != userId)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        // upload to versioned s3...
        publication.Title = req.Title ?? publication.Title;
        publication.UploadedAt = DateTimeConverter.RemoveTimeZone(DateTime.UtcNow);
        await libraryPublicationRepository.UpdateAsync(publication, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateLibraryPublicationRequest(
    Guid Id,
    string? Title,
    IFormFile? PreviewPicture,
    IFormFile? Publication
);