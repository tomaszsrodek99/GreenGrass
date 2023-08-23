using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenGrassAPI.Models
{
    public class Plant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public DateTime DateAdded { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[]? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? CareInstructions { get; set; }
        //PARAMETRY KWIATKA XD
        [Required]
        public int TemperatureRangeMin { get; set; }
        [Required]
        public int TemperatureRangeMax { get; set; }
        [Required]
        public int HumidityRangeMin { get; set; }
        [Required]
        public int HumidityRangeMax { get; set; }
        public string? SoilType { get; set; }
        public string? Prunning { get; set; }
        public string Lighting { get; set; } = null!;
        public string? Bursting { get; set; }
        public string? PottedSuggestions { get; set; }
        //NOTIFICATION
        [Required]
        public int WateringFrequency { get; set; }
        [Required]
        public int FertilizingFrequency { get; set; }
        [Required]
        public int RepottingFrequency { get; set; }
        //DEPENDENCIES
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        public int? NotificationId { get; set; }
        public Notification? Notification { get; set; }
    }
}