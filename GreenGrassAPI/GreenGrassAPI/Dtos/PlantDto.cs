using GreenGrassAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenGrassAPI.Dtos
{
    public class PlantDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2"), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        [Display(Name = "Nazwa")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Data dodania")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Zdjęcie")]
        public string ImageUrl { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Porady")]
        public string? CareInstructions { get; set; }
        //PARAMETRY Rośliny
        [Display(Name = "Zakres temperatury min.")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public int TemperatureRangeMin { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Zakres temperatury max.")]
        public int TemperatureRangeMax { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Zakres wilgotności min.")]
        public int HumidityRangeMin { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Zakres wilgotności max.")]
        public int HumidityRangeMax { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone.")]
        [Display(Name = "Podłoże"), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        public string SoilType { get; set; } = string.Empty;
        [Display(Name = "Przycinanie")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone."), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        public string Prunning { get; set; } = string.Empty;
        [Display(Name = "Oświetlenie")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone."), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        public string Lighting { get; set; } = string.Empty;
        [Display(Name = "Rozsadzanie")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone."), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        public string Bursting { get; set; } = string.Empty;
        [Display(Name = "Sugestie doniczkowe")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Tylko litery są dozwolone."), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        public string PottedSuggestions { get; set; } = string.Empty;
        //NOTIFICATION
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Częstotliwość podlewania (dni)")]
        public int WateringFrequency { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Częstotliwość nawożenia (dni)")]
        public int FertilizingFrequency { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Częstotliwość przesadzania (dni)")]
        public int RepottingFrequency { get; set; }
        //Dependencies
        public int UserId { get; set; }
        //WALIDACJE
        public static ValidationResult ValidateTemperatureRange(Plant plant)
        {
            if (plant.TemperatureRangeMax <= plant.TemperatureRangeMin)
            {
                return new ValidationResult("Górna granica zakresu temperatury musi być większa od dolnej granicy.", new[] { "TemperatureRangeMax" });
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateHumidityRange(Plant plant)
        {
            if (plant.HumidityRangeMax <= plant.HumidityRangeMin)
            {
                return new ValidationResult("Górna granica zakresu wilgotności musi być większa od dolnej granicy.", new[] { "HumidityRangeMax" });
            }
            return ValidationResult.Success;
        }
    }
    public enum LightingType
    {
        [Display(Name = "Pełne słońce")]
        FullSun,

        [Display(Name = "Częściowe słońce")]
        PartialSun,

        [Display(Name = "Częściowy cień")]
        PartialShade,

        [Display(Name = "Pełen cień")]
        FullShade
    }
}