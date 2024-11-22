namespace POEPART2CMCSFINAL.Models
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; }
        public string LecturerName { get; set; }
        public DateTime DateIssued { get; set; }
        public string ClaimPeriod { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
