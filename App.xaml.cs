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
        private static DbContextOptions<AppDbContext>? _dbContextOptions;

        public static AppDbContext DbContext => _dbContext ??= new AppDbContext(_dbContextOptions!);
        public static DataService DataService => _dataService ??= new DataService(DbContext);
        public static ThemeService ThemeService => ThemeService.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                // Initialize database context options
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlite("Data Source=vinokurnya.db");
                _dbContextOptions = optionsBuilder.Options;

                // Initialize services
                _dbContext = new AppDbContext(_dbContextOptions);
                _dataService = new DataService(_dbContext);

                // Initialize database and theme
                _dbContext.Database.EnsureCreated();

                // Ensure ThemeService is initialized before using it
                var themeService = ThemeService.Instance;
                themeService.LoadThemePreference();
                themeService.ApplyTheme(themeService.CurrentTheme);

                // Load sample data asynchronously
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _dataService.EnsureSampleDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке образцовых данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
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