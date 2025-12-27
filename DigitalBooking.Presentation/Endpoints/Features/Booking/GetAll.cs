using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Booking;

public sealed class GetAllBookedEventEndpoint(IBookingRepository bookedEventRepository)
    : EndpointWithoutRequest<GetAllBookedEventResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<BookingGroup>();

        Description(b => b
            .WithName("GetAllBookedEvent")
            .WithTags(RouteGroups.Booking)
            .Produces<GetAllBookedEventResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await bookedEventRepository.GetAllAsync(ct);
        var items = result
            .Select(booking => new ReadBookedEventResponse
            (
                Id: booking.Id,
                ModifiedAt: booking.ModifiedAt,
                MeetingLink: booking.MeetingLink,
                BookedStartTime: booking.BookingStart,
                BookedEndTime: booking.BookingEnd,
                BookingNote: booking.BookingNote,
                GroupName: booking.Lesson?.Group.GetDisplayName(),
                ClassroomTitle: booking.Classroom.Title,
                ClassroomOwnerDepartment: booking.Classroom.OwnerDepartment.Title,
                ClassroomType: booking.Classroom.ClassroomType.TypeName,
                BookerPersonId: booking.PersonBookedId,
                BookerPersonFullname: booking.PersonBooked.Fullname,
                ResponsiblePersonId: booking.ResponsiblePersonId,
                ResponsiblePersonFullname: booking.ResponsiblePerson.Fullname,
                LessonName: booking.Lesson?.Discipline.Title,
                LessonTeacherFullname: booking.Lesson?.Teacher.Fullname,
                AssetId: booking.BookingAssetId,
                CancelledByUserId: booking.CancelledById,
                CancelledByUserFullname: booking.CancelledBy?.Fullname
            ))
            .ToList();

        await Send.OkAsync(new GetAllBookedEventResponse(items), ct);
    }
}

public sealed record GetAllBookedEventResponse(List<ReadBookedEventResponse> Items);
