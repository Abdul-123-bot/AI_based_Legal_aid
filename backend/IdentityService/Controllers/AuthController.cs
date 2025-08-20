using System.Security.Claims;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace IdentityService.Controllers;

[ApiController]
[Route("auth")]
[Authorize]
public class AuthController: ControllerBase{

    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context){
        _context = context;
    }

    [HttpGet("me")]
    [RequiredScope("access_as_user")]
    public async Task<IActionResult> GetMyProfile(){
        var userId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        var userEmail = User.FindFirstValue(ClaimTypes.Upn) ?? User.FindFirstValue("preferred_username");

        if( string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail) ){
            return Unauthorized("User identifier not found in token");
        }

        var userProfile = await _context.UserProfiles.FindAsync(Guid.Parse(userId));

        if(userProfile == null){
            
            userProfile = new UserProfile{
                Id = Guid.Parse(userId),
                Email = userEmail,
                FirstName = User.FindFirstValue(ClaimTypes.GivenName),
                LastName = User.FindFirstValue(ClaimTypes.Surname)
            };
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
        }

        return Ok( new {
            Id = userProfile.Id,
            Email = userProfile.Email,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            Source = "User profile from Azure SQL."
        });
    }

    [HttpPost("refresh")]
    public IActionResult RefreshToken(){
        return Ok(new {
             Message = "Token refresh is handled by the client with Azure AD." 
        });
    }

}