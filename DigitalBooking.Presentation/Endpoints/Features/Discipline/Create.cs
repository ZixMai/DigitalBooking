using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Discipline;

public sealed class CreateDisciplineEndpoint(IDisciplineRepository disciplineRepository) : Endpoint<CreateDisciplineRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<DisciplineGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("CreateDiscipline")
            .WithTags(RouteGroups.Discipline));
    }

    internal sealed class Validator : Validator<CreateDisciplineRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(200).WithMessage("Превышена максимальная длина названия");
        }
    }

    public override async Task HandleAsync(CreateDisciplineRequest req, CancellationToken ct)
    {
        var entity = await disciplineRepository.GetByTitleAsync(req.Title, ct);
        if (entity is not null)
        {
            AddError("Already exists.");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        await disciplineRepository.CreateAsync(new Domain.Discipline { Title = req.Title }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateDisciplineRequest(string Title);
