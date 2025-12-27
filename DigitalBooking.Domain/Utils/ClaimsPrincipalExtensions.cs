using System.Security.Claims;

namespace DigitalBooking.Domain.Utils;

public static class ClaimsPrincipalExtensions
{
    public static (Guid, Role?) GetIdAndRole(this ClaimsPrincipal user)
    {
        Role? role = null;
        if (!Enum.TryParse<Role>(user.FindFirst(ClaimTypes.Role)?.Value, ignoreCase: true, out var parsed))
        {
            role = parsed;
        }
        return (Guid.Parse(user.FindFirst("UserId")!.Value), role);
    } 
}