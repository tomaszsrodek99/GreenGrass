using GreenGrassAPI.Models;
using GreenGrassAPI.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenGrassAPI.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public int PlantId { get; set; }
        public PlantView? Plant { get; set; }
        [Display(Name = "Poprzednio podlano")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LastWateringDate { get; set; }
        [Display(Name = "Należy podlać dnia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public DateTime NextWateringDate { get; set; }
        [Display(Name = "Poprzednio nawożono")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LastFertilizingDate { get; set; }
        [Display(Name = "Należy nawieżć dnia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public DateTime NextFertilizingDate { get; set; }
    }
}