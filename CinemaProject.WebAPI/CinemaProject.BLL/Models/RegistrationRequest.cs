using System.ComponentModel.DataAnnotations;

namespace CinemaProject.BLL.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
