using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinokurnyaWpf.Data
{
    public class Note
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ProcessStage Stage { get; set; } = ProcessStage.Braga;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsFavorite { get; set; } = false;
        public string Tags { get; set; } = string.Empty; // Comma-separated tags
    }

    public enum ProcessStage
    {
        Braga = 1,
        Distillation,
        Aging,
        Tasting,
        Other
    }

    public class DensityDataPoint
    {
        public double Temperature { get; set; } // °C
        public double Density { get; set; } // kg/m³
        public double ABV { get; set; } // % Alcohol by Volume
    }

    public class DistillationResult
    {
        public double Volume { get; set; } // Total volume in liters
        public double HeadsVolume { get; set; } // Heads volume in liters
        public double HeartsVolume { get; set; } // Hearts volume in liters
        public double TailsVolume { get; set; } // Tails volume in liters
        public double HeadsPercentage { get; set; } // Percentage of heads
        public double HeartsPercentage { get; set; } // Percentage of hearts
        public double TailsPercentage { get; set; } // Percentage of tails
        public double EstimatedABV { get; set; } // Estimated ABV of hearts
    }

    public class CalculationHistory
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string CalculationType { get; set; } = string.Empty; // Dilution, Distillation, Speed, Temperature
        public string ParametersJson { get; set; } = "{}";
        public string ResultJson { get; set; } = "{}";
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; } = string.Empty;
    }
}