using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using POEPART2CMCSFINAL.Controllers;
using POEPART2CMCSFINAL.Models;
using POEPART2CMCSFINAL.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POEPART2CMCSFINAL.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly Mock<ClaimContext> _mockClaimContext;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockClaimContext = new Mock<ClaimContext>();

            // Initialize mock claim context with test data
            var mockUsers = new List<Users>
            {
                new Users { ID = 1, username = "lecturer", password = "pass", role = "Lecturer" },
                new Users { ID = 2, username = "manager", password = "pass", role = "Manager" }
            }.AsQueryable();

            var mockUsersDbSet = new Mock<DbSet<Users>>();
            mockUsersDbSet.As<IQueryable<Users>>().Setup(m => m.Provider).Returns(mockUsers.Provider);
            mockUsersDbSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(mockUsers.Expression);
            mockUsersDbSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(mockUsers.ElementType);
            mockUsersDbSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(mockUsers.GetEnumerator());

            _mockClaimContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

            _controller = new HomeController(_mockLogger.Object, _mockEnvironment.Object, _mockClaimContext.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public void Login_ValidCredentials_RedirectsToDashboard()
        {
            // Arrange
            var model = new LoginViewModel { username = "lecturer", password = "pass" };

            // Act
            var result = _controller.Login(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var model = new LoginViewModel { username = "wronguser", password = "wrongpass" };

            // Act
            var result = _controller.Login(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid login details", result?.ViewData["Message"]);
        }

        [Fact]
        public void Login_InvalidModelState_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("username", "Username is required");

            var model = new LoginViewModel { username = "", password = "pass" };

            // Act
            var result = _controller.Login(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }
    }
}
