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

            FavoriteRecipes = new ObservableCollection<Recipe>();
            RecentNotes = new ObservableCollection<Note>();

            ToggleThemeCommand = new RelayCommand(ToggleTheme);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            IsLoading = true;

            try
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