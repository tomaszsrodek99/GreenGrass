using GreenGrassAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GreenGrassAPI.Dtos
{
    public class PlantDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2"), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Data dodania")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Zdjęcie")]
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Opis")]
        public string? Description { get; set; }

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
        [Display(Name = "Podłoże")]
        public string? SoilType { get; set; }
        [Display(Name = "Przycinanie")]
        public string? Prunning { get; set; }
        [Display(Name = "Oświetlenie")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Lighting { get; set; } = string.Empty;
        [Display(Name = "Rozsadzanie")]
        public string? Bursting { get; set; }
        [Display(Name = "Sugestie doniczkowe")]
        public string? PottedSuggestions { get; set; }
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
        public static ValidationResult ValidateTemperatureRange(PlantDto plant)
        {
            if (plant.TemperatureRangeMax >= plant.TemperatureRangeMin)
            {
                return new ValidationResult("Górna granica zakresu temperatury musi być większa od dolnej granicy.", new[] { "TemperatureRangeMax" });
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateHumidityRange(PlantDto plant)
        {
            if (plant.HumidityRangeMax >= plant.HumidityRangeMin)
            {
                return new ValidationResult("Górna granica zakresu wilgotności musi być większa od dolnej granicy.", new[] { "HumidityRangeMax" });
            }
            return ValidationResult.Success;
        }
    }
    public enum LightingType
    {
        [Display(Name = "Pełne słońce")]
        Słoneczne,

        [Display(Name = "Częściowe słońce")]
        Rozproszone,

        [Display(Name = "Częściowy cień")]
        Zacienione,

        [Display(Name = "Pełen cień")]
        Cień
    }
}