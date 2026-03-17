using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.Services;
using VinokurnyaWpf.Helpers;

namespace VinokurnyaWpf.ViewModels
{
    public enum CalculatorType
    {
        Dilution,
        Distillation,
        Speed,
        Temperature
    }

    public class CalculatorViewModel : BindableBase
    {
        private readonly CalculationService _calculationService;
        private readonly DataService _dataService;

        private CalculatorType _selectedCalculator;
        private bool _isLoading;

        public CalculatorType SelectedCalculator
        {
            get => _selectedCalculator;
            set => SetProperty(ref _selectedCalculator, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        // Dilution calculator properties
        private double _volumeMl;
        private double _currentABV;
        private double _targetABV;
        private double _temperatureC = 20;

        public double VolumeMl
        {
            get => _volumeMl;
            set => SetProperty(ref _volumeMl, value);
        }

        public double CurrentABV
        {
            get => _currentABV;
            set => SetProperty(ref _currentABV, value);
        }

        public double TargetABV
        {
            get => _targetABV;
            set => SetProperty(ref _targetABV, value);
        }

        public double TemperatureC
        {
            get => _temperatureC;
            set => SetProperty(ref _temperatureC, value);
        }

        // Dilution result
        private string _dilutionResult;
        public string DilutionResult
        {
            get => _dilutionResult;
            set => SetProperty(ref _dilutionResult, value);
        }

        // Distillation calculator properties
        private double _distillationVolume;
        private double _distillationABV;

        public double DistillationVolume
        {
            get => _distillationVolume;
            set => SetProperty(ref _distillationVolume, value);
        }

        public double DistillationABV
        {
            get => _distillationABV;
            set => SetProperty(ref _distillationABV, value);
        }

        // Distillation result
        private string _distillationResult;
        public string DistillationResult
        {
            get => _distillationResult;
            set => SetProperty(ref _distillationResult, value);
        }

        // Speed calculator properties
        private double _speedABV;
        private double _speedTemperature;
        private double _equipmentFactor;

        public double SpeedABV
        {
            get => _speedABV;
            set => SetProperty(ref _speedABV, value);
        }

        public double SpeedTemperature
        {
            get => _speedTemperature;
            set => SetProperty(ref _speedTemperature, value);
        }

        public double EquipmentFactor
        {
            get => _equipmentFactor;
            set => SetProperty(ref _equipmentFactor, value);
        }

        private string _speedResult;
        public string SpeedResult
        {
            get => _speedResult;
            set => SetProperty(ref _speedResult, value);
        }

        private string _speedIntensity;
        public string SpeedIntensity
        {
            get => _speedIntensity;
            set => SetProperty(ref _speedIntensity, value);
        }

        private string _speedRecommendation;
        public string SpeedRecommendation
        {
            get => _speedRecommendation;
            set => SetProperty(ref _speedRecommendation, value);
        }

        // Temperature calculator properties
        private double _temperatureABV;
        private double _atmosphericPressure = 1013.25;

        public double TemperatureABV
        {
            get => _temperatureABV;
            set => SetProperty(ref _temperatureABV, value);
        }

        public double AtmosphericPressure
        {
            get => _atmosphericPressure;
            set => SetProperty(ref _atmosphericPressure, value);
        }

        private string _temperatureResult;
        public string TemperatureResult
        {
            get => _temperatureResult;
            set => SetProperty(ref _temperatureResult, value);
        }

        public ObservableCollection<CalculationHistory> CalculationHistory { get; }

        public ICommand CalculateDilutionCommand { get; }
        public ICommand CalculateDistillationCommand { get; }
        public ICommand CalculateSpeedCommand { get; }
        public ICommand CalculateTemperatureCommand { get; }
        public ICommand SaveCalculationCommand { get; }

        public CalculatorViewModel()
        {
            _calculationService = new CalculationService();
            _dataService = App.DataService;

            CalculationHistory = new ObservableCollection<CalculationHistory>();

            CalculateDilutionCommand = new RelayCommand(CalculateDilution, CanCalculateDilution);
            CalculateDistillationCommand = new RelayCommand(CalculateDistillation, CanCalculateDistillation);
            CalculateSpeedCommand = new RelayCommand(CalculateSpeed, CanCalculateSpeed);
            CalculateTemperatureCommand = new RelayCommand(CalculateTemperature, CanCalculateTemperature);
            SaveCalculationCommand = new RelayCommand(SaveCalculation, CanSaveCalculation);

            // Load calculation history asynchronously after initialization
            _ = LoadCalculationHistoryAsync();
        }

        #region Dilution Calculator

        private bool CanCalculateDilution()
        {
            return VolumeMl > 0 && CurrentABV > 0 && CurrentABV < 100 && TargetABV > 0 && TargetABV < CurrentABV;
        }

        private void CalculateDilution()
        {
            IsLoading = true;
            try
            {
                var result = _calculationService.CalculateDilution(VolumeMl, CurrentABV, TargetABV, TemperatureC);
                DilutionResult = $"Вода: {result.waterMl:F2} мл\nИтоговый объем: {result.finalVolumeMl:F2} мл\nКрепость: {result.correctedABV:F1}%";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Distillation Calculator

        private bool CanCalculateDistillation()
        {
            return DistillationVolume > 0 && DistillationABV > 0;
        }

        private void CalculateDistillation()
        {
            IsLoading = true;
            try
            {
                var result = _calculationService.CalculateFractionalDistillation(
                    DistillationVolume, DistillationABV);
                DistillationResult = $"Головы: {result.HeadsVolume:F2} л ({result.HeadsPercentage:F1}%)\n" +
                                    $"Тело: {result.HeartsVolume:F2} л ({result.HeartsPercentage:F1}%)\n" +
                                    $"Хвосты: {result.TailsVolume:F2} л ({result.TailsPercentage:F1}%)\n" +
                                    $"Оценочная крепость тела: {result.EstimatedABV:F1}%";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Speed Calculator

        private bool CanCalculateSpeed()
        {
            return SpeedABV > 0 && SpeedTemperature >= -20 && SpeedTemperature <= 60;
        }

        private void CalculateSpeed()
        {
            IsLoading = true;
            try
            {
                var (drops, recommendation, intensity) = _calculationService.CalculateOptimalSpeed(
                    SpeedABV, SpeedTemperature, EquipmentFactor);
                SpeedResult = $"Оптимальная скорость: {drops:F1} капель/сек\nИнтенсивность: {intensity}\n\n{recommendation}";
                SpeedIntensity = intensity;
                SpeedRecommendation = recommendation;
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Temperature Calculator

        private bool CanCalculateTemperature()
        {
            return TemperatureABV > 0 && AtmosphericPressure >= 900 && AtmosphericPressure <= 1100;
        }

        private void CalculateTemperature()
        {
            IsLoading = true;
            try
            {
                var (boilingPoint, optimalTemperature, distillationTemperature) = 
                    _calculationService.CalculateTemperatureRegime(TemperatureABV, AtmosphericPressure);
                TemperatureResult = $"Температура кипения воды: {boilingPoint:F1}°C\n" +
                                    $"Оптимальная температура перегонки: {optimalTemperature:F1}°C\n" +
                                    $"Температура в змеевике: {distillationTemperature:F1}°C";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Save Calculation

        private bool CanSaveCalculation()
        {
            return SelectedCalculator != CalculatorType.Dilution || !string.IsNullOrEmpty(DilutionResult) ||
                   SelectedCalculator != CalculatorType.Distillation || !string.IsNullOrEmpty(DistillationResult) ||
                   SelectedCalculator != CalculatorType.Speed || !string.IsNullOrEmpty(SpeedResult) ||
                   SelectedCalculator != CalculatorType.Temperature || !string.IsNullOrEmpty(TemperatureResult);
        }

        private async void SaveCalculation()
        {
            try
            {
                var calculation = new CalculationHistory
                {
                    CalculationType = SelectedCalculator.ToString()
                };

                string parameters = "";
                string result = "";

                switch (SelectedCalculator)
                {
                    case CalculatorType.Dilution:
                        parameters = $"Объем: {VolumeMl:F2} мл, Текущая крепость: {CurrentABV:F1}%, Целевая крепость: {TargetABV:F1}%, Температура: {TemperatureC:F1}°C";
                        result = DilutionResult ?? "";
                        break;
                    case CalculatorType.Distillation:
                        parameters = $"Объем: {DistillationVolume:F2} л, Крепость: {DistillationABV:F1}%";
                        result = DistillationResult ?? "";
                        break;
                    case CalculatorType.Speed:
                        parameters = $"Крепость: {SpeedABV:F1}%, Температура: {SpeedTemperature:F1}°C, Фактор оборудования: {EquipmentFactor:F1}";
                        result = SpeedResult ?? "";
                        break;
                    case CalculatorType.Temperature:
                        parameters = $"Крепость: {TemperatureABV:F1}%, Давление: {AtmosphericPressure:F1} гПа";
                        result = TemperatureResult ?? "";
                        break;
                }

                calculation.ParametersJson = parameters;
                calculation.ResultJson = result;

                await _dataService.AddCalculationHistoryAsync(calculation);
                CalculationHistory.Add(calculation);

                System.Windows.MessageBox.Show("Расчет сохранен", "Успешно", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        private async Task LoadCalculationHistoryAsync()
        {
            try
            {
                if (_dataService != null)
                {
                    var history = await _dataService.GetCalculationHistoryAsync(50);
                    foreach (var item in history)
                    {
                        CalculationHistory.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                // Silent fail
            }
        }
    }
}