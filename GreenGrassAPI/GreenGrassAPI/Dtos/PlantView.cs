using System.ComponentModel.DataAnnotations;
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
        public string ImageUrl { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Porady")]
        public string CareInstructions { get; set; } = string.Empty;

        [Display(Name = "Zakres temperatury min.")]
        public int TemperatureRangeMin { get; set; }

        [Display(Name = "Zakres temperatury max.")]
        public int TemperatureRangeMax { get; set; }

        [Display(Name = "Zakres wilgotności min.")]
        public int HumidityRangeMin { get; set; }

        [Display(Name = "Zakres wilgotności max.")]
        public int HumidityRangeMax { get; set; }

        [Display(Name = "Podłoże")]
        public string SoilType { get; set; } = string.Empty;

        [Display(Name = "Oświetlenie")]
        public string Lighting { get; set; } = string.Empty;

        [Display(Name = "Przycinanie")]
        public string Prunning { get; set; } = string.Empty;

        [Display(Name = "Rozsadzanie")]
        public string Bursting { get; set; } = string.Empty;

        [Display(Name = "Sugestie doniczkowe")]
        public string PottedSuggestions { get; set; } = string.Empty;

        [Display(Name = "Częstotliwość podlewania (dni)")]
        public int WateringFrequency { get; set; }

        [Display(Name = "Częstotliwość nawożenia (dni)")]
        public int FertilizingFrequency { get; set; }

        [Display(Name = "Częstotliwość przesadzania (dni)")]
        public int RepottingFrequency { get; set; }

        [Display(Name = "Ostatnio podlano")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LastWateringDate { get; set; }

        [Display(Name = "Ostatnio nawożono")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LastFertilizingDate { get; set; }

        [Display(Name = "Należy podlać")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]

        public DateTime NextWateringDate { get; set; }

        [Display(Name = "Należy nawieżć")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime NextFertilizingDate { get; set; }

        [Display(Name = "Pozostało dni do podlania")]
        public int DaysUntilWatering { get; set; }

        [Display(Name = "Pozostało dni do nawożenia")]
        public int DaysUntilFertilizing { get; set; }
        public int UserId { get; set; }
    }
}