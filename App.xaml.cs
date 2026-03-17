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

        public static AppDbContext DbContext => _dbContext ??= new AppDbContext();
        public static DataService DataService => _dataService ??= new DataService(DbContext);
        public static ThemeService ThemeService => ThemeService.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize services
            _dbContext = new AppDbContext();
            _dataService = new DataService(_dbContext);

            // Initialize database and theme
            _dbContext.Database.EnsureCreated();
            ThemeService.LoadThemePreference();
            ThemeService.ApplyTheme(ThemeService.CurrentTheme);

            // Load sample data asynchronously
            _ = Task.Run(async () =>
            {
                await _dataService.EnsureSampleDataAsync();
            });
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