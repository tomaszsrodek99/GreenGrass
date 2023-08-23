using GreenGrassAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace GreenGrassAPI.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [MaxLength(128, ErrorMessage = "Maksymalna długośc to 128 znaków."), Required(ErrorMessage = "Pole jest wymagane."), EmailAddress(ErrorMessage = "Pole musi być formatu email - xyz@gmail.com.")]
        public string Email { get; set; } = null!;
        //public string Role { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

    }
}