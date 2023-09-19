using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenGrassAPI.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PlantId { get; set; }
        public Plant? Plant { get; set; }
        public DateTime LastWateringDate { get; set; }
        public DateTime NextWateringDate { get; set; }
        public int WateringPeriod { get; set; }
        public DateTime LastFertilizingDate { get; set; }
        public DateTime NextFertilizingDate { get; set; }
        public int FertilizingPeriod { get; set; }
    }
}