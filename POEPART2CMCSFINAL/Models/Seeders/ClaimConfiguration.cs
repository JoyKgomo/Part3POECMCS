using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace POEPART2CMCSFINAL.Models.Seeders
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            // Define primary key
            builder.HasKey(b => b.Id);

            // Configure foreign key relationship
            builder.HasOne(p => p.Users)
                   .WithMany(x => x.Claims)
                   .HasForeignKey(p => p.UserID); // Ensure this matches your model

            // Seed data
            builder.HasData
            (
                new Claim
                {
                    Id = 1,
                    UserID = 1, // Must align with seeded user ID
                    DateClaimed = DateTime.Now,
                    status = "Pending",
                    HourlyRate = 2000,
                    HoursWorked = 34,
                    AmountDue = 2400
                },
                new Claim
                {
                    Id = 2,
                    UserID = 2, // Must align with seeded user ID
                    DateClaimed = DateTime.Now,
                    status = "pending",
                    HourlyRate = 2000,
                    HoursWorked = 34,
                    AmountDue = 2400
                }
            );
        }
    }
}
