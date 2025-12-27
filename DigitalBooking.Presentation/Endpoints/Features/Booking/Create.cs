using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using FluentValidation;

namespace DigitalBooking.Presentation.Endpoints.Features.Booking;

public sealed class CreateBookedEventEndpoint(
    IBookingRepository bookedEventRepository,
    IUserRepository userRepository,
    IClassroomRepository classroomRepository,
    ILessonsRepository lessonsRepository
) : Endpoint<CreateBookedEventRequest>
{
    public override void Configure()
    {
        Post("/");
        Group<BookingGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("CreateBookedEvent")
            .WithTags(RouteGroups.Booking));
    }

    internal sealed class Validator : Validator<CreateBookedEventRequest>
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

    public override async Task HandleAsync(CreateBookedEventRequest req, CancellationToken ct)
    {
        var (userId, _) = User.GetIdAndRole();
        var user = await userRepository.GetUserAsync(userId, ct);
        var classroom = await classroomRepository.GetAsync(req.ClassRoomId, ct);
        if (req.ResponsiblePersonId is not null)
        {
            var respPerson = await userRepository.GetUserAsync(req.ResponsiblePersonId.Value, ct);
            if (respPerson is null) { AddError("Не найден пользователь отвественный за мероприятие"); }
        }
        if (req.LessonId is not null)
        {
            var lesson = await lessonsRepository.GetAsync(req.LessonId.Value, ct);
            if (lesson is null) { AddError("Не найдено занятие"); }
        }
        if (classroom is null) { AddError("Не найдена аудитория"); }
        if (user!.DepartmentId != classroom?.OwnerDepartmentId)
        {
            AddError("Невозможно забронировать аудиторию чужой кафедры");
        }
        if (req.BookingAssetId is not null)
        {
            var asset = await bookedEventRepository.GetAssetAsync(req.BookingAssetId.Value, ct);
            if (asset is null) { AddError("Не найден шаблон брони"); }
        }
        // todo сделать валидацию по поводу того что на это время и аудиторию ничего нет

        if (ValidationFailed)
        {
            await Send.ErrorsAsync(cancellation: ct);
        }
        
        await bookedEventRepository.CreateAsync(new Domain.Booking
        {
            ClassroomId = req.ClassRoomId,
            PersonBookedId = userId,
            ResponsiblePersonId = req.ResponsiblePersonId ?? userId,
            LessonId = req.LessonId,
            MeetingLink = req.MeetingLink,
            BookingAssetId = req.BookingAssetId,
            BookingStart = req.BookingStart,
            BookingEnd = req.BookingEnd,
            BookingNote = req.BookingNote
        }, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record CreateBookedEventRequest(
    long ClassRoomId,
    Guid? ResponsiblePersonId,
    long? LessonId,
    string? MeetingLink,
    long? BookingAssetId,
    DateTime BookingStart,
    DateTime BookingEnd,
    string? BookingNote
);