using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Booking;

public sealed class ReadBookedEventEndpoint(IBookingRepository bookedEventRepository) : Endpoint<ReadBookedEventRequest, ReadBookedEventResponse>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<BookingGroup>();

        Description(b => b
            .WithName("ReadBookedEvent")
            .WithTags(RouteGroups.Booking)
            .Produces<ReadBookedEventResponse>());
    }

    public override async Task HandleAsync(ReadBookedEventRequest req, CancellationToken ct)
    {
        var result = await bookedEventRepository.GetAsync(req.Id, ct);
        if (result == null) { await Send.NotFoundAsync(ct); return; }
        await Send.OkAsync(
            new ReadBookedEventResponse
            (
                Id: result.Id,
                ModifiedAt: result.ModifiedAt,
                MeetingLink: result.MeetingLink,
                BookedStartTime: result.BookingStart,
                BookedEndTime: result.BookingEnd,
                BookingNote: result.BookingNote,
                GroupName: result.Lesson?.Group.GetDisplayName(),
                ClassroomTitle: result.Classroom.Title,
                ClassroomOwnerDepartment: result.Classroom.OwnerDepartment.Title,
                ClassroomType: result.Classroom.ClassroomType.TypeName,
                BookerPersonId: result.PersonBookedId,
                BookerPersonFullname: result.PersonBooked.Fullname,
                ResponsiblePersonId: result.ResponsiblePersonId,
                ResponsiblePersonFullname: result.ResponsiblePerson.Fullname,
                LessonName: result.Lesson?.Discipline.Title,
                LessonTeacherFullname: result.Lesson?.Teacher.Fullname,
                AssetId: result.BookingAssetId,
                CancelledByUserId: result.CancelledById,
                CancelledByUserFullname: result.CancelledBy?.Fullname
            ), ct);
    }
}

public sealed record ReadBookedEventRequest([FromRoute] long Id);
public sealed record ReadBookedEventResponse(
    long Id,
    DateTime ModifiedAt,
    string? MeetingLink,
    DateTime BookedStartTime,
    DateTime BookedEndTime,
    string? BookingNote,
    string? GroupName,
    string ClassroomTitle,
    string ClassroomOwnerDepartment,
    string ClassroomType,
    Guid BookerPersonId,
    string BookerPersonFullname,
    Guid ResponsiblePersonId,
    string ResponsiblePersonFullname,
    string? LessonName,
    string? LessonTeacherFullname,
    long? AssetId,
    Guid? CancelledByUserId,
    string? CancelledByUserFullname
);