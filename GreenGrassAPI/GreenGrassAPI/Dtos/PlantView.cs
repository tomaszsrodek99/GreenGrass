﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GreenGrassAPI.Dtos
{
    public class PlantView
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; } = null!;

        [Display(Name = "Data dodania")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Zdjęcie")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Zakres temperatury min.")]
        public int TemperatureRangeMin { get; set; }

        [Display(Name = "Zakres temperatury max.")]
        public int TemperatureRangeMax { get; set; }

        [Display(Name = "Zakres wilgotności min.")]
        public int HumidityRangeMin { get; set; }

        [Display(Name = "Zakres wilgotności max.")]
        public int HumidityRangeMax { get; set; }

        [Display(Name = "Podłoże")]
        public string? SoilType { get; set; }

        [Display(Name = "Oświetlenie")]
        public string Lighting { get; set; } = null!;

        [Display(Name = "Częstotliwość podlewania (dni)")]
        public int WateringFrequency { get; set; }

        [Display(Name = "Częstotliwość nawożenia (dni)")]
        public int FertilizingFrequency { get; set; }

        [Display(Name = "Częstotliwość przesadzania (dni)")]
        public int RepottingFrequency { get; set; }


        //NOTIFICATION

        [Display(Name = "Ostatnio podlano")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LastWateringDate { get; set; }

        [Display(Name = "Ostatnio nawożono")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LastFertilizingDate { get; set; }

        [Display(Name = "Należy podlać")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]

        public DateTime? NextWateringDate { get; set; }

        [Display(Name = "Należy nawieżć")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NextFertilizingDate { get; set; } 

        [Display(Name = "Podlewanie")]
        public int? DaysUntilWatering {
            get
            {
                if (NotificationDto == null || NotificationDto.LastWateringDate.Date == DateTime.Today)
                {
                    return null;
                }
                else
                {
                    return (int?)(NotificationDto.NextWateringDate.Date - DateTime.Now.Date).TotalDays;
                }
            }
            set
            {

            }
        }
        [NotMapped]
        public string? DaysUntilWateringInfo
        {
            get
            {
                if (DaysUntilWatering == null)
                {
                    return null;
                }
                else if (NotificationDto.LastWateringDate.Date == DateTime.Today)
                {
                    return "Podlano dzisiaj.";
                }
                else if(DaysUntilWatering > 0)
                {
                    return $"Należy podlać za: {DaysUntilWatering} dni.";
                }
                else
                {
                    return "Podlej teraz";
                }
            }
        }

        [Display(Name = "Nawożenie")]
        public int? DaysUntilFertilizing
        {
            get
            {
                if (NotificationDto == null || NotificationDto.LastFertilizingDate.Date == DateTime.Today)
                {
                    return null;
                }
                else
                {
                    return (int?)(NotificationDto.NextFertilizingDate.Date - DateTime.Now.Date).TotalDays;
                }
            }
            set
            {

            }
        }
        [NotMapped]
        public string? DaysUntilFertilizingInfo
        {
            get
            {
                if (DaysUntilFertilizing == null)
                {
                    return null;
                }
                else if (NotificationDto.LastFertilizingDate.Date == DateTime.Today)
                {
                    return "Nawieziono dzisiaj.";
                }
                else if (DaysUntilFertilizing > 0)
                {
                    return $"Należy nawieżć za: {DaysUntilFertilizing} dni.";
                }
                else
                {
                    return "Nawieź teraz";
                }
            }
        }

        public int UserId { get; set; }
        public int? NotificationId { get; set; }
        public NotificationDto? NotificationDto { get; set; }
    }
}