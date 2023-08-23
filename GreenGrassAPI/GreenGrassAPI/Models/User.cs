using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenGrassAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        [Required]
        public string Email { get; set; } = null!;
        //[Required]
        //public string Role { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public ICollection<Plant>? Plants { get; set; }
    }
}