using System.ComponentModel.DataAnnotations;

namespace privateCinema.DTOs.LoginDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "emlail is required")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; } = "";

        public string Username { get; set; }="";
        public string PhoneNumber { get; set; } = "";

    }
}
