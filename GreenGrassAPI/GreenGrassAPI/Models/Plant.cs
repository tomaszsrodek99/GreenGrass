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
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CareInstructions { get; set; } = string.Empty;
        //PARAMETRY KWIATKA XD
        [Required]
        public int TemperatureRangeMin { get; set; }
        [Required]
        public int TemperatureRangeMax { get; set; }
        [Required]
        public int HumidityRangeMin { get; set; }
        [Required]
        public int HumidityRangeMax { get; set; }
        public string SoilType { get; set; } = string.Empty;
        public string Prunning { get; set; } = string.Empty;
        public string Lighting { get; set; } = string.Empty;
        public string Bursting { get; set; } = string.Empty;
        public string PottedSuggestions { get; set; } = string.Empty;
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
        public User User { get; set; } = null!;
        public int? NotificationId { get; set; }
        public Notification? Notification { get; set; }
    }
}