using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEPART2CMCSFINAL.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }
    }
}
