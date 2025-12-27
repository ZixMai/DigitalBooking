using System.Threading;
using System.Threading.Tasks;
using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Discipline;

public sealed class UpdateDisciplineEndpoint(IDisciplineRepository disciplineRepository) : Endpoint<Domain.Discipline>
{
    public override void Configure()
    {
        Put("/");
        Group<DisciplineGroup>();
        Description(b => b
            .WithName("UpdateDiscipline")
            .WithTags(RouteGroups.Discipline));
        Roles(nameof(Role.ADMIN));
    }

    internal sealed class Validator : Validator<Domain.Discipline>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название должно быть не пустым.")
                .MaximumLength(200).WithMessage("Превышена максимальная длина названия");
        }
    }

    public override async Task HandleAsync(Domain.Discipline reqEntity, CancellationToken ct)
    {
        if (await disciplineRepository.GetAsync(reqEntity.Id, ct) is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await disciplineRepository.UpdateAsync(reqEntity, ct);
        await Send.NoContentAsync(ct);
    }
}
