using System.Security.Claims;

namespace CampusPlacementManagement.API.Helpers;

public static class CurrentUserHelper
{
    public static string? GetUserId(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetEmail(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email);
    }
}