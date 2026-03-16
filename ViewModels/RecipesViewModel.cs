using System;
using System.Collections.ObjectModel;
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
    public class RecipesViewModel : BindableBase
    {
        private readonly DataService _dataService;

        private string _searchQuery = "";
        private string _selectedCategory = "Все";
        private bool _showFavoritesOnly = false;
        private bool _isLoading;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public bool ShowFavoritesOnly
        {
            get => _showFavoritesOnly;
            set => SetProperty(ref _showFavoritesOnly, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<Recipe> Recipes { get; }

        public ObservableCollection<string> Categories { get; }
        public ObservableCollection<Recipe> FilteredRecipes { get; }

        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ShowFavoritesCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ExportRecipesCommand { get; }

        public RecipesViewModel()
        {
            _dataService = App.DataService;

            Recipes = new ObservableCollection<Recipe>();
            Categories = new ObservableCollection<string> { "Все", "Виски", "Бурбон", "Настойки" };
            FilteredRecipes = new ObservableCollection<Recipe>();

            SearchCommand = new RelayCommand(Search);
            FilterCommand = new RelayCommand(Filter);
            ShowFavoritesCommand = new RelayCommand(ShowFavorites);
            ToggleFavoriteCommand = new RelayCommand<Guid>(ToggleFavorite);
            ExportRecipesCommand = new RelayCommand(ExportRecipes);

            LoadRecipesAsync();
        }

        private async void LoadRecipesAsync()
        {
            IsLoading = true;
            try
            {
                var allRecipes = await _dataService.GetAllRecipesAsync();
                Recipes.Clear();
                foreach (var recipe in allRecipes)
                {
                    Recipes.Add(recipe);
                }
                FilterRecipes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading recipes: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Search()
        {
            FilterRecipes();
        }

        private void Filter()
        {
            FilterRecipes();
        }

        private void ShowFavorites()
        {
            ShowFavoritesOnly = !ShowFavoritesOnly;
            FilterRecipes();
        }

        private async void ToggleFavorite(Guid recipeId)
        {
            try
            {
                var result = await _dataService.ToggleFavoriteAsync(recipeId);

                var recipe = Recipes.FirstOrDefault(r => r.Id == recipeId);
                if (recipe != null)
                {
                    result = ToggleFavoriteResult.Success(!recipe.IsFavorite);
                }

                if (result == ToggleFavoriteResult.Success)
                {
                    System.Windows.MessageBox.Show("Рецепт сохранен в избранное", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterRecipes()
        {
            FilteredRecipes.Clear();

            var filtered = Recipes.AsEnumerable();

            // Filter by category
            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "Все")
            {
                filtered = filtered.Where(r => r.Category == SelectedCategory);
            }

            // Filter by search query
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(r => r.Name.ToLower().Contains(SearchQuery.ToLower()) ||
                                              r.Category.ToLower().Contains(SearchQuery.ToLower()));
            }

            // Filter by favorites
            if (ShowFavoritesOnly)
            {
                filtered = filtered.Where(r => r.IsFavorite);
            }

            foreach (var recipe in filtered)
            {
                FilteredRecipes.Add(recipe);
            }
        }

        private async void ExportRecipes()
        {
            try
            {
                var recipesToExport = ShowFavoritesOnly ? Recipes.Where(r => r.IsFavorite) : Recipes;

                if (!recipesToExport.Any())
                {
                    System.Windows.MessageBox.Show("Нет рецептов для экспорта", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var json = System.Text.Json.JsonSerializer.Serialize(recipesToExport, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON файлы (*.json)|*.json|Текстовые файлы (*.txt)|*.txt",
                    FileName = "vinokurnya_recipes.json"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    await File.WriteAllTextAsync(saveDialog.FileName, json);
                    System.Windows.MessageBox.Show("Рецепты успешно экспортированы", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public enum ToggleFavoriteResult
    {
        Success,
        NotFound
    }
}