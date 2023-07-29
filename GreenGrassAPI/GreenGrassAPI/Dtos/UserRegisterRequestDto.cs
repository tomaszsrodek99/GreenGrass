using System.ComponentModel.DataAnnotations;

namespace GreenGrassAPI.Dtos
{
    public class UserRegisterRequestDto
    {
        [MaxLength(128, ErrorMessage = "Maksymalna długośc to 128 znaków."), Required(ErrorMessage = "Pole jest wymagane."), EmailAddress(ErrorMessage = "Pole musi być formatu email - xyz@gmail.com.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Hasło")]
        [MinLength(6, ErrorMessage = "Minimalna długość to 6 znaków.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane."), Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        [Display(Name = "Potwierdż hasło")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}