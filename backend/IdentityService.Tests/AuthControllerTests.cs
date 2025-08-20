using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using IdentityService.Controllers;
using IdentityService.Data;
using IdentityService.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Tests
{
    public class AuthControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetMyProfile_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new AuthController(context);

            var userId = Guid.NewGuid().ToString();
            var email = "test@example.com";

            var claims = new[]
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userId),
                new Claim(ClaimTypes.Upn, email),
                new Claim(ClaimTypes.GivenName, "Test"),
                new Claim(ClaimTypes.Surname, "User")
            };

            var identity = new ClaimsIdentity(claims, "mock");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act
            var result = await controller.GetMyProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        } // ✅ closes method
    }     // ✅ closes class
}         // ✅ closes namespace
