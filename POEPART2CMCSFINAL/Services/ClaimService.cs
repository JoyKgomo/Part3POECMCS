using Microsoft.AspNetCore.Mvc;
using POEPART2CMCSFINAL.Models;

namespace POEPART2CMCSFINAL.Services
{
    public class ClaimService
    {
        ClaimContext claimContext;
        public ClaimService(ClaimContext claimContext)
        {
            this.claimContext = claimContext;
            this.claimContext.Database.EnsureCreated();
        }
        [HttpPost]
        public int AddNewClaim(Claim claim)
        {
            // Check if claimContext is not null
            if (claimContext == null)
            {
                Console.WriteLine("Database context is null.");
                return 0; // Return 0 to indicate failure
            }

            // Getting the lecturer defined rate per hour based on UserID (or other relevant identifier)
            var lecturer = claimContext.Users.FirstOrDefault(x => x.ID == claim.UserID); // Ensure claim has a UserID
            if (lecturer == null)
            {
                Console.WriteLine("Lecturer not found for UserID: " + claim.UserID);
                return 0; // Return 0 if the user is not found
            }

            // Here you might define the hourly rate for a claim
            //claim.HourlyRate = lecturer.HourlyRate; // Assuming Users has HourlyRate property
            claim.AmountDue = claim.HoursWorked * claim.HourlyRate; // Calculate total fee

            // Add the claim to the database
            claimContext.Claims.Add(claim);
            claimContext.SaveChanges(); // Save changes to the database

            return claim.Id; // Return the new claim ID
        }


        public int UpdateClaim(Claim claim)
        {
            //logic to update to claim to you database
            var _claim = claimContext.Claims.FirstOrDefault(x => x.Id == claim.Id);
            if (_claim != null)
            {
                double totalFee = claim.HoursWorked * claim.HourlyRate;
                _claim.AmountDue = totalFee;
                _claim.DateClaimed = claim.DateClaimed;
                _claim.UserID = claim.UserID;
                _claim.status = claim.status;
                //add fields
                //claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<ClaimItemModel> GetAllClaimsForUser(int personId)
        {
            
            // Fetch claims for the user
            var claims = (from c in claimContext.Claims
                          join u in claimContext.Users on c.UserID equals u.ID // Ensure correct property name
                          where c.UserID == personId
                          select new ClaimItemModel
                          {
                              Id = c.Id,
                              DateClaimed = c.DateClaimed,
                              Hours = c.HoursWorked,
                              rate = c.HourlyRate,
                              Name = u.Name,
                              TotalFee = c.AmountDue,
                              status = c.status
                          }).ToList();

            if (!claims.Any())
            {
                Console.WriteLine("No claims found for user ID: " + personId);
            }

            return claims.OrderByDescending(x => x.DateClaimed)
                         .ThenBy(x => x.status)
                         .ToList();
        }

    }
}
