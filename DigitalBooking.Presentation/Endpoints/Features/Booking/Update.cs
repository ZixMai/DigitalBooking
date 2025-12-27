using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Booking;

public sealed class UpdateBookedEventEndpoint(
    IBookingRepository bookedEventRepository,
    IClassroomRepository classroomRepository,
    IUserRepository userRepository
) : Endpoint<UpdateBookedEventRequest>
{
    public override void Configure()
    {
        Put("/");
        Group<BookingGroup>();
        Roles(nameof(Role.ADMIN));
        Description(b => b
            .WithName("UpdateBookedEvent")
            .WithTags(RouteGroups.Booking));
    }

    internal sealed class Validator : Validator<Domain.Booking>
    {
        public Validator()
        {
            RuleFor(x => x.MeetingLink)
                .NotEmpty().When(x => x.MeetingLink != null).WithMessage("Ссылка должна быть не пустой.")
                .MaximumLength(1000).When(x => x.MeetingLink != null)
                .WithMessage("Превышена максимальная длина ссылки");
            RuleFor(x => x.BookingEnd)
                .GreaterThan(x => x.BookingStart)
                .WithMessage("Время окончания должно быть позже времени начала.")
                .Must((req, end) => end - req.BookingStart <= TimeSpan.FromHours(13))
                .WithMessage("Длительность бронирования не должна превышать 13 часов.");
        }
    }
    
    public override async Task HandleAsync(UpdateBookedEventRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        var entity = await bookedEventRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            await Send.NotFoundAsync(ct); 
            return;
        }
        if ((entity.PersonBookedId != userId && role != Role.ADMIN) || entity.CancelledById is not null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var responsiblePerson = await userRepository.GetUserAsync(req.ResponsiblePersonId, ct);
        if (responsiblePerson is null) { AddError("Не найден ответственный пользователь"); }
        var classroom = await classroomRepository.GetAsync(req.ClassroomId, ct);
        if (classroom is null) { AddError("Аудитория не найдена"); }

        if (ValidationFailed)
        {
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }
        
        entity = new Domain.Booking
        {
            Id = req.Id,
            ClassroomId = req.ClassroomId,
            PersonBookedId = userId,
            ResponsiblePersonId = req.ResponsiblePersonId,
            ModifiedAt = DateTimeConverter.RemoveTimeZone(DateTime.UtcNow),
            LessonId = entity.LessonId,
            MeetingLink = req.MeetingLink,
            BookingAssetId = entity.BookingAssetId,
            BookingStart = req.BookingStart,
            BookingEnd = req.BookingEnd,
            BookingNote = req.BookingNote,
            GroupId = entity.GroupId,
        };
        await bookedEventRepository.UpdateAsync(entity, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record UpdateBookedEventRequest(
    long Id,
    long ClassroomId,
    Guid ResponsiblePersonId,
    string? MeetingLink,
    DateTime BookingStart,
    DateTime BookingEnd,
    string? BookingNote
);