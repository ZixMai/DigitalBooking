using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBooking.Presentation.Endpoints.Features.Classroom;

public sealed class ReadClassroomEndpoint(IClassroomRepository classroomRepository) : Endpoint<ReadClassroomRequest, ReadClassroomResponse>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<ClassroomGroup>();

        Description(b => b
            .WithName("ReadClassroom")
            .WithTags(RouteGroups.Classroom)
            .Produces<ReadClassroomResponse>());
    }

    public override async Task HandleAsync(ReadClassroomRequest req, CancellationToken ct)
    {
        var result = await classroomRepository.GetAsync(req.Id, ct);
        if (result == null) { await Send.NotFoundAsync(ct); return; }
        await Send.OkAsync(
            new ReadClassroomResponse
            (
                Id: result.Id,
                Title: result.Title,
                OwnerDepartment: result.OwnerDepartment.Title,
                BookingSlotsLimit: result.BookingSlotsLimit,
                Capacity: result.Capacity,
                ClassroomType: result.ClassroomType.TypeName,
                CreatedAt: result.CreatedAt
            ), ct);
    }
}

public sealed record ReadClassroomRequest([FromRoute] long Id);
public sealed record ReadClassroomResponse(
    long Id,
    string Title,
    string OwnerDepartment,
    short BookingSlotsLimit,
    short? Capacity,
    string ClassroomType,
    DateTime CreatedAt
);
