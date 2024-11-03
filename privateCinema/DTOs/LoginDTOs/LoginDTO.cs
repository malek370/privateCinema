using System.ComponentModel.DataAnnotations;

namespace Ath.DTOs.LoginDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; } = "";
    }
}
