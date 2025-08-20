using System.Security.Claims;
using System.Security.Principal;

namespace IdentityService.Tests;

public static class ClaimsPrincipalHelper
{
    public static ClaimsPrincipal CreateTestPrincipal(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userId),
            new Claim(ClaimTypes.Upn, email)
        };

        var identity = new ClaimsIdentity(claims, "mock");
        return new ClaimsPrincipal(identity);
    }
}
