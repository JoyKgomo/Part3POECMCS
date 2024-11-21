using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEPART2CMCSFINAL.Models
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string status { get; set; }
        public DateTime DateClaimed { get; set; }
        public int HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double AmountDue { get; set; }
        [NotMapped]
        public IFormFile Document {  get; set; }
        
        public virtual Users Users { get; set; }
    }
}
