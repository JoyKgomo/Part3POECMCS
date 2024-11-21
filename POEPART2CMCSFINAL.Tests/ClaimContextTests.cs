using Microsoft.EntityFrameworkCore;
using POEPART2CMCSFINAL.Models;
using POEPART2CMCSFINAL.Services;
using System;
using System.Linq;
using Xunit;

namespace POEPART2CMCSFINAL.Tests
{
    public class ClaimContextTests
    {
        private DbContextOptions<ClaimContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<ClaimContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Can_Add_And_Retrieve_User()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            // Act
            using (var context = new ClaimContext(options))
            {
                var user = new Users { ID = 1, username = "testuser", password = "password", role = "Lecturer" };
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Assert
            using (var context = new ClaimContext(options))
            {
                var user = context.Users.FirstOrDefault(u => u.ID == 1);
                Assert.NotNull(user);
                Assert.Equal("testuser", user.username);
            }
        }

        [Fact]
        public void Can_Add_And_Retrieve_Claim()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            // Act
            using (var context = new ClaimContext(options))
            {
                var claim = new Claim
                {
                    Id = 1,
                    UserID = 1,
                    HoursWorked = 10,
                    HourlyRate = 50,
                    DateClaimed = DateTime.Now,
                    status = "Pending"
                };
                context.Claims.Add(claim);
                context.SaveChanges();
            }

            // Assert
            using (var context = new ClaimContext(options))
            {
                var claim = context.Claims.FirstOrDefault(c => c.Id == 1);
                Assert.NotNull(claim);
                Assert.Equal(10, claim.HoursWorked);
                Assert.Equal("Pending", claim.status);
            }
        }

        [Fact]
        public void Can_Add_And_Retrieve_Document()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            // Act
            using (var context = new ClaimContext(options))
            {
                var document = new Document
                {
                    Id = 1,
                    FileName = "testfile.pdf",
                    DateUploaded = DateTime.Now,
                    //Id = 1
                };
                context.Documents.Add(document);
                context.SaveChanges();
            }

            // Assert
            using (var context = new ClaimContext(options))
            {
                var document = context.Documents.FirstOrDefault(d => d.Id == 1);
                Assert.NotNull(document);
                Assert.Equal("testfile.pdf", document.FileName);
            }
        }
    }
}
