using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.Services;
using VinokurnyaWpf.Helpers;

namespace VinokurnyaWpf.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly DataService _dataService;
        private readonly CalculationService _calculationService;
        private readonly AppDbContext _dbContext;

        private int _selectedTab;
        private bool _isLoading;

        public int SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ToggleThemeCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public ObservableCollection<Recipe> FavoriteRecipes { get; }
        public ObservableCollection<Note> RecentNotes { get; }

        public MainViewModel()
        {
            _dataService = App.DataService;
            _calculationService = new CalculationService();
            _dbContext = App.DbContext;

            FavoriteRecipes = new ObservableCollection<Recipe>();
            RecentNotes = new ObservableCollection<Note>();

            ToggleThemeCommand = new RelayCommand(ToggleTheme);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            // Load data asynchronously after initialization
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;

            try
            {
                if (_dataService != null)
                {
                    var recipes = await _dataService.GetFavoriteRecipesAsync();
                    foreach (var recipe in recipes)
                    {
                        FavoriteRecipes.Add(recipe);
                    }

                    var notes = await _dataService.GetAllNotesAsync();
                    foreach (var note in notes.Take(5))
                    {
                        RecentNotes.Add(note);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ToggleTheme()
        {
            App.ThemeService.ToggleTheme();
        }

        private void ShowSettings()
        {
            System.Windows.MessageBox.Show("Настройки будут доступны в будущих версиях", "Внимание",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        public void Cleanup()
        {
            IsLoading = false;
            FavoriteRecipes.Clear();
            RecentNotes.Clear();
        }

        private void ToggleTheme()
        {
            App.ThemeService.ToggleTheme();
        }

        private void ShowSettings()
        {
            // TODO: Open settings window
            System.Windows.MessageBox.Show("Настройки будут доступны в будущих версиях", "Внимание", 
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        public void Cleanup()
        {
            IsLoading = false;
            FavoriteRecipes.Clear();
            RecentNotes.Clear();
        }
    }
}