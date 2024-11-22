using System.ComponentModel.DataAnnotations;

namespace POEPART2CMCSFINAL.Models
{
    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
       
    }
}
