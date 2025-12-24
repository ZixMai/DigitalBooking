using DigitalBooking.Application.Abstractions;
using DigitalBooking.Application.Services;
using DigitalBooking.Domain;
using DigitalBooking.Presentation.Groups;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity.Data;

namespace DigitalBooking.Presentation.Endpoints.Features.Auth;

using FastEndpoints;

public class RefreshEndpoint(
    IUserRepository userRepository,
    JwtService jwtService
) : Endpoint<RefreshRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("/refresh");
        Group<AuthGroup>();
        AllowAnonymous();
        Policies(ApiPolicies.IsTokenRefresh);
        
        Description(b => b
            .WithName("Refresh")
            .WithTags(RouteGroups.Auth)
            .Produces<TokenResponse>());
    }

    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var user = await userRepository.GetUserAsync(Guid.Parse(User.FindFirst("UserId")!.Value), ct);
        if (user == null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        await Send.OkAsync(jwtService.GenerateTokenPair(user), ct);
    }
}