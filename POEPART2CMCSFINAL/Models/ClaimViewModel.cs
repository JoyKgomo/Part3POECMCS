using Microsoft.AspNetCore.Mvc.Rendering;

namespace POEPART2CMCSFINAL.Models
{
    public class ClaimViewModel
    {
        public DateTime ClaimPeriod { get; set; }
        public int HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double TotalFee => HoursWorked * HourlyRate; // Calculated total fee
        public IFormFile? Document { get; set; }  // File upload
        public string? AdditionalNotes { get; set; }  // Any extra notes
    }
}
