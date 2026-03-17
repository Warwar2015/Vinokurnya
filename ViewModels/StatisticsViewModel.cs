using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.Services;
using VinokurnyaWpf.Helpers;

namespace VinokurnyaWpf.ViewModels
{
    public class StatisticsViewModel : BindableBase
    {
        private readonly DataService _dataService;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<Recipe> TotalRecipes { get; }
        public ObservableCollection<Recipe> FavoriteRecipes { get; }
        public ObservableCollection<Note> TotalNotes { get; }

        public string TotalRecipesCount => TotalRecipes.Count.ToString();
        public string FavoriteRecipesCount => FavoriteRecipes.Count.ToString();
        public string TotalNotesCount => TotalNotes.Count.ToString();
        public string AverageRating => TotalRecipes.Any() ? TotalRecipes.Average(r => r.Rating).ToString("F1") : "0.0";
        public string MostUsedCategory => TotalRecipes.Any() ? GetMostUsedCategory() : "Нет данных";

        public StatisticsViewModel()
        {
            _dataService = App.DataService;

            TotalRecipes = new ObservableCollection<Recipe>();
            FavoriteRecipes = new ObservableCollection<Recipe>();
            TotalNotes = new ObservableCollection<Note>();

            // Load statistics asynchronously after initialization
            _ = LoadStatisticsAsync();
        }

        public async Task LoadStatisticsAsync()
        {
            IsLoading = true;
            try
            {
                if (_dataService != null)
                {
                    var allRecipes = await _dataService.GetAllRecipesAsync();
                    var favoriteRecipes = await _dataService.GetFavoriteRecipesAsync();
                    var allNotes = await _dataService.GetAllNotesAsync();

                TotalRecipes.Clear();
                FavoriteRecipes.Clear();
                TotalNotes.Clear();

                foreach (var recipe in allRecipes)
                {
                    TotalRecipes.Add(recipe);
                }

                foreach (var recipe in favoriteRecipes)
                {
                    FavoriteRecipes.Add(recipe);
                }

                foreach (var note in allNotes)
                {
                    TotalNotes.Add(note);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading statistics: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string GetMostUsedCategory()
        {
            if (!TotalRecipes.Any())
                return "Нет данных";

            var categoryCounts = TotalRecipes
                .GroupBy(r => r.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return categoryCounts ?? "Неизвестно";
        }
    }
}