using System.ComponentModel.DataAnnotations;

namespace GreenGrassAPI.Dtos
{
    public class UserLoginRequestDto
    {
        [MaxLength(128, ErrorMessage = "Maksymalna długośc to 128 znaków."), Required(ErrorMessage = "Pole jest wymagane."), EmailAddress(ErrorMessage = "Pole musi być formatu email - xyz@gmail.com.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;
    }
}