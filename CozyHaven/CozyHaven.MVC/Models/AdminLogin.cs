using System.ComponentModel.DataAnnotations;

namespace CozyHaven.MVC.Models
{
    public class AdminLogin
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
