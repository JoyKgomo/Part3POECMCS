using Microsoft.EntityFrameworkCore;
using POEPART2CMCSFINAL.Models;
using POEPART2CMCSFINAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POEPART2CMCSFINAL.Tests
{
    public class ClaimServiceTests
    {
        private DbContextOptions<ClaimContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<ClaimContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AddNewClaim_ShouldReturnClaimId_WhenClaimIsAddedSuccessfully()
        {
            // Arrange
            var options = CreateInMemoryOptions();
            var claim = new Claim
            {
                UserID = 1,
                HoursWorked = 10,
                HourlyRate = 20,
                DateClaimed = DateTime.Now
            };

            using (var context = new ClaimContext(options))
            {
                context.Users.Add(new Users { ID = 1, Name = "Test User" });
                context.SaveChanges();
                var service = new ClaimService(context);

                // Act
                var claimId = service.AddNewClaim(claim);

                // Assert
                Assert.True(claimId > 0);
            }
        }

        [Fact]
        public void AddNewClaim_ShouldReturnZero_WhenUserNotFound()
        {
            // Arrange
            var options = CreateInMemoryOptions();
            var claim = new Claim
            {
                UserID = 99, // Non-existent user ID
                HoursWorked = 10,
                HourlyRate = 20,
                DateClaimed = DateTime.Now
            };

            using (var context = new ClaimContext(options))
            {
                var service = new ClaimService(context);

                // Act
                var claimId = service.AddNewClaim(claim);

                // Assert
                Assert.Equal(0, claimId);
            }
        }

        [Fact]
        public void UpdateClaim_ShouldReturnClaimId_WhenClaimIsUpdatedSuccessfully()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            using (var context = new ClaimContext(options))
            {
                var claim = new Claim
                {
                    Id = 1,
                    UserID = 1,
                    HoursWorked = 5,
                    HourlyRate = 20,
                    DateClaimed = DateTime.Now,
                    AmountDue = 100
                };
                context.Claims.Add(claim);
                context.SaveChanges();

                var service = new ClaimService(context);

                // Act
                claim.HoursWorked = 8; // Update the hours worked
                service.UpdateClaim(claim);

                // Assert
                var updatedClaim = context.Claims.FirstOrDefault(c => c.Id == claim.Id);
                Assert.NotNull(updatedClaim);
                Assert.Equal(8 * claim.HourlyRate, updatedClaim.AmountDue);
            }
        }

        [Fact]
        public void GetAllClaimsForUser_ShouldReturnClaims_WhenClaimsExistForUser()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            using (var context = new ClaimContext(options))
            {
                var user = new Users { ID = 1, Name = "Test User" };
                context.Users.Add(user);

                context.Claims.AddRange(
                    new Claim { UserID = 1, HoursWorked = 10, HourlyRate = 20, DateClaimed = DateTime.Now.AddDays(-1), status = "Pending" },
                    new Claim { UserID = 1, HoursWorked = 5, HourlyRate = 20, DateClaimed = DateTime.Now, status = "Approved" }
                );
                context.SaveChanges();

                var service = new ClaimService(context);

                // Act
                var claims = service.GetAllClaimsForUser(1);

                // Assert
                Assert.Equal(2, claims.Count);
                Assert.Equal("Test User", claims.First().Name);
                Assert.Equal(200, claims.First().TotalFee); // 10 * 20
            }
        }

        [Fact]
        public void GetAllClaimsForUser_ShouldReturnEmptyList_WhenNoClaimsExistForUser()
        {
            // Arrange
            var options = CreateInMemoryOptions();

            using (var context = new ClaimContext(options))
            {
                context.Users.Add(new Users { ID = 1, Name = "Test User" });
                context.SaveChanges();

                var service = new ClaimService(context);

                // Act
                var claims = service.GetAllClaimsForUser(2); // Non-existent user ID

                // Assert
                Assert.Empty(claims);
            }
        }
    }
}
