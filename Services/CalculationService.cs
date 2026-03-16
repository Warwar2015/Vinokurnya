using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VinokurnyaWpf.Data;

namespace VinokurnyaWpf.Services
{
    public class CalculationService
    {
        // Плотность спирта при разных температурах (g/cm³)
        private static readonly Dictionary<double, double> AlcoholDensityTable = new Dictionary<double, double>
        {
            { -10, 0.8128 }, { -5, 0.8110 }, { 0, 0.8095 }, { 5, 0.8080 },
            { 10, 0.8064 }, { 15, 0.8048 }, { 20, 0.8032 }, { 25, 0.8015 },
            { 30, 0.7998 }, { 35, 0.7981 }, { 40, 0.7963 }, { 45, 0.7945 },
            { 50, 0.7926 }
        };

        private static readonly double[] StandardAlcoholConcentrations = { 40, 50, 60, 70, 80, 90, 96 };

        // 1. Расчет разбавления спирта с учетом температуры
        public (double waterMl, double finalVolumeMl, double correctedABV) CalculateDilution(
            double volumeMl, double currentABV, double targetABV, double temperatureC = 20)
        {
            // Коррекция плотности на температуру
            double currentDensity = GetDensityAtTemperature(currentABV, temperatureC);
            double targetDensity = GetDensityAtTemperature(targetABV, 20); // При 20°C для сравнения

            // Расчет воды по плотности
            double waterMl = volumeMl * (currentDensity / targetDensity - 1);
            double finalVolume = volumeMl + waterMl;

            return (waterMl, finalVolume, targetABV);
        }

        // 2. Расчет дробной перегонки
        public DistillationResult CalculateFractionalDistillation(
            double volumeLiters, double initialABV, double headsPercent = 12, double tailsPercent = 12)
        {
            double headsVolume = volumeLiters * (headsPercent / 100.0);
            double tailsVolume = volumeLiters * (tailsPercent / 100.0);
            double heartsVolume = volumeLiters - headsVolume - tailsVolume;
            double heartsPercentage = (heartsVolume / volumeLiters) * 100;

            // Оценка крепости hearts (средняя между head и tail)
            double estimatedHeartsABV = (initialABV * 0.85 + initialABV * 0.70) / 2;

            return new DistillationResult
            {
                Volume = volumeLiters,
                HeadsVolume = headsVolume,
                HeartsVolume = heartsVolume,
                TailsVolume = tailsVolume,
                HeadsPercentage = headsPercent,
                HeartsPercentage = (double)Math.Round(heartsPercentage, 2),
                TailsPercentage = tailsPercent,
                EstimatedABV = (double)Math.Round(estimatedHeartsABV, 2)
            };
        }

        // 3. Расчет оптимальной скорости отбора
        public (double optimalDropsPerSecond, string recommendation, string intensity) CalculateOptimalSpeed(
            double abv, double temperatureC, double equipmentFactor = 1.0)
        {
            // Базовая скорость (капель/секунда) в зависимости от крепости
            double baseDropsPerSecond = Math.Max(1, abv / 10);

            // Коррекция на температуру
            double temperatureCorrection = 1.0 - ((temperatureC - 20) / 100.0);
            temperatureCorrection = Math.Max(0.8, Math.Min(1.2, temperatureCorrection));

            double optimalDropsPerSecond = baseDropsPerSecond * temperatureCorrection * equipmentFactor;

            // Рекомендация по интенсивности
            string intensity;
            if (optimalDropsPerSecond > 4)
            {
                intensity = "🔴 Быстро";
                recommendation = "Высокая скорость. Рекомендуется использовать аппарат с мощным нагревом и хорошей системой охлаждения.";
            }
            else if (optimalDropsPerSecond >= 2)
            {
                intensity = "🟢 Норма";
                recommendation = "Оптимальная скорость. Подходит для большинства самогонных аппаратов.";
            }
            else
            {
                intensity = "🟡 Медленно";
                recommendation = "Низкая скорость. Может потребовать более длительного времени перегонки.";
            }

            return ((double)Math.Round(optimalDropsPerSecond, 1), recommendation, intensity);
        }

        // 4. Расчет температурных режимов с учетом атмосферного давления
        public (double boilingPoint, double optimalTemperature, double distillationTemperature) CalculateTemperatureRegime(
            double abv, double atmosphericPressure = 1013.25) // Давление в гПа
        {
            // Коррекция температуры кипения воды по давлению (формула Клапейрона-Клаузиуса)
            double pressureCorrection = 1 - ((atmosphericPressure - 1013.25) / 1013.25) * 0.034;
            pressureCorrection = Math.Max(0.95, Math.Min(1.05, pressureCorrection));

            double boilingPoint = 100 * pressureCorrection; // Температура кипения воды (°C)
            double optimalTemperature = 78 + (abv / 2); // Оптимальная температура перегонки
            double distillationTemperature = 95 - (abv / 10); // Температура в змеевике

            return ((double)Math.Round(boilingPoint, 1), (double)Math.Round(optimalTemperature, 1), (double)Math.Round(distillationTemperature, 1));
        }

        // Получить плотность спирта при заданной температуре и крепости
        private double GetDensityAtTemperature(double abv, double temperature)
        {
            // Используем интерполяцию между известными точками
            var points = AlcoholDensityTable
                .Where(kvp => kvp.Key <= temperature)
                .OrderByDescending(kvp => kvp.Key)
                .ToList();

            if (points.Count == 0)
                return 0.8095; // Базовая плотность при 20°C

            if (points.Count == 1 || points[0].Key == temperature)
                return points[0].Value;

            var lower = points[0];
            var upper = points[1];

            double t = temperature;
            double t1 = lower.Key;
            double t2 = upper.Key;
            double r1 = lower.Value;
            double r2 = upper.Value;

            // Линейная интерполяция
            double density = r1 + (t - t1) * (r2 - r1) / (t2 - t1);
            return (double)Math.Round(density, 4);
        }

        // Получить стандартные концентрации для конкретной температуры
        public double[] GetStandardConcentrations(double temperature = 20)
        {
            double[] adjustedConcentrations = (double[])StandardAlcoholConcentrations.Clone();

            // Коррекция крепости на температуру (спирт меньше расширяется при низких температурах)
            double temperatureFactor = 1.0 - ((temperature - 20) / 500.0);
            temperatureFactor = Math.Max(0.9, Math.Min(1.0, temperatureFactor));

            for (int i = 0; i < adjustedConcentrations.Length; i++)
            {
                adjustedConcentrations[i] *= temperatureFactor;
                adjustedConcentrations[i] = (double)Math.Round(adjustedConcentrations[i], 1);
            }

            return adjustedConcentrations;
        }
    }
}