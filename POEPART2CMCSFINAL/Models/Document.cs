using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POEPART2CMCSFINAL.Models
{
    public class Document
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string FileName { get; set; }
    }
}
