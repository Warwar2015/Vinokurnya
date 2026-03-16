using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VinokurnyaWpf.Data
{
    public class Recipe
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Whisky, Bourbon, Infusion, etc.
        public string Subcategory { get; set; } = string.Empty;
        public int Difficulty { get; set; } = 1; // 1-5
        public int TimeDays { get; set; } = 0;
        public double YieldLiters { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public string ImagePath { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // JSON-stored collections
        public string IngredientsJson { get; set; } = "[]";
        public string StepsJson { get; set; } = "[]";
        public string DistillationParametersJson { get; set; } = "{}";

        [NotMapped]
        public List<RecipeIngredient> Ingredients
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<RecipeIngredient>>(IngredientsJson) 
                   ?? new List<RecipeIngredient>();
            set => IngredientsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<RecipeStep> Steps
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<RecipeStep>>(StepsJson)
                   ?? new List<RecipeStep>();
            set => StepsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public DistillationParameters DistillationParameters
        {
            get => System.Text.Json.JsonSerializer.Deserialize<DistillationParameters>(DistillationParametersJson)
                   ?? new DistillationParameters();
            set => DistillationParametersJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

    public class RecipeIngredient
    {
        public string Name { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }

    public class RecipeStep
    {
        public int Order { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? DurationMinutes { get; set; }
        public double? Temperature { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class DistillationParameters
    {
        public double InitialABV { get; set; }
        public double TargetABV { get; set; }
        public double HeadsPercentage { get; set; }
        public double HeartsPercentage { get; set; }
        public double TailsPercentage { get; set; }
        public double? Temperature { get; set; }
        public double? AtmosphericPressure { get; set; }
        public string Equipment { get; set; } = string.Empty;
    }
}