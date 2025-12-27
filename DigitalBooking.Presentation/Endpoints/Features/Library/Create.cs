using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Library;

public sealed class CreateLibraryPublicationEndpoint(ILibraryRepository libraryRepository)
    : Endpoint<CreateLibraryRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<LibraryGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        AllowFileUploads();
        Description(b => b
            .WithName("CreateLibraryPublication")
            .WithTags(RouteGroups.Library));
    }

    internal sealed class Validator : Validator<CreateLibraryRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(1000).WithMessage("Превышена максимальная длина названия");
        }
    }

    public override async Task HandleAsync(CreateLibraryRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        var previewS3Key = Guid.NewGuid();
        var publicationS3Key = Guid.NewGuid();
        // upload to versioned s3...
        await libraryRepository.CreateAsync(new Domain.LibraryPublication
        {
            Title = req.Title,
            CreatorId = userId,
            PreviewKey = previewS3Key,
            ContentKey = publicationS3Key
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateLibraryRequest(
    string Title,
    IFormFile PreviewPicture,
    IFormFile Publication
);