using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using DigitalBooking.Domain.Utils;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Booking;

public sealed class DeleteBookedEventEndpoint(IBookingRepository bookedEventRepository) : Endpoint<DeleteBookedEventRequest>
{
    public override void Configure()
    {
        Delete("/{id}");
        Group<BookingGroup>();
        Roles(nameof(Role.ADMIN), nameof(Role.TEACHER));
        Description(b => b
            .WithName("DeleteBookedEvent")
            .WithTags(RouteGroups.Booking));
    }

    public override async Task HandleAsync(DeleteBookedEventRequest req, CancellationToken ct)
    {
        var (userId, role) = User.GetIdAndRole();
        var entity = await bookedEventRepository.GetAsync(req.Id, ct);
        if (entity is null) {
            await Send.NotFoundAsync(ct); 
            return;
        }
        if (entity.PersonBookedId != userId && role != Role.ADMIN)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        
        entity.CancelledById = userId;
        entity.ModifiedAt = DateTimeConverter.RemoveTimeZone(DateTime.UtcNow);
        await bookedEventRepository.UpdateAsync(entity, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed record DeleteBookedEventRequest([FromRoute] long Id);