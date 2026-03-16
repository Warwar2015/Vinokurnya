using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.Services;
using VinokurnyaWpf.ViewModels;
using VinokurnyaWpf.Views;

namespace VinokurnyaWpf
{
    public partial class App : Application
    {
        private static AppDbContext? _dbContext;
        private static DataService? _dataService;
        private static ThemeService? _themeService;

        public static AppDbContext DbContext => _dbContext ??= new AppDbContext();
        public static DataService DataService => _dataService ??= new DataService(DbContext);
        public static ThemeService ThemeService => _themeService ??= new ThemeService();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize database
            InitializeDatabase();
            
            // Set theme from settings or system default
            ThemeService.InitializeTheme();
            
            // Load sample data if needed
            DataService.EnsureSampleData();
        }

        private void InitializeDatabase()
        {
            try
            {
                DbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}", 
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Save theme preference
            ThemeService.SaveThemePreference();
            
            // Clean up
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }
}