using System.ComponentModel.DataAnnotations;

namespace CinemaProject.BLL.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
