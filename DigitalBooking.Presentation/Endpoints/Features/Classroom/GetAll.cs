using DigitalBooking.Application.Abstractions;
using FastEndpoints;
using DigitalBooking.Presentation.Groups;

namespace DigitalBooking.Presentation.Endpoints.Features.Classroom;

public sealed class GetAllClassroomEndpoint(IClassroomRepository classroomRepository)
    : EndpointWithoutRequest<GetAllClassroomResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<ClassroomGroup>();

        Description(b => b
            .WithName("GetAllClassroom")
            .WithTags(RouteGroups.Classroom)
            .Produces<GetAllClassroomResponse>());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await classroomRepository.GetAllAsync(ct);
        var items = result
            .Select(classroom => new ReadClassroomResponse
            (
                Id: classroom.Id,
                Title: classroom.Title,
                OwnerDepartment: classroom.OwnerDepartment.Title,
                BookingSlotsLimit: classroom.BookingSlotsLimit,
                Capacity: classroom.Capacity,
                ClassroomType: classroom.ClassroomType.TypeName,
                CreatedAt: classroom.CreatedAt
            ))
            .ToList();

        await Send.OkAsync(new GetAllClassroomResponse(items), ct);
    }
}

public sealed record GetAllClassroomResponse(List<ReadClassroomResponse> Items);
