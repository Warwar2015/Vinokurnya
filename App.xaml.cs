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

        private void App_Startup(object sender, StartupEventArgs e)
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

                // Initialize database
                _dbContext.Database.EnsureCreated();

                // Ensure ThemeService is initialized
                ThemeService.LoadThemePreference();
                ThemeService.ApplyTheme(ThemeService.CurrentTheme);

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
                MessageBox.Show($"Ошибка при запуске приложения:\n{ex.Message}\n\n{ex.StackTrace}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show($"Необработанное исключение:\n{e.Exception.Message}\n\n{e.Exception.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                // Catch any errors in the error handler itself
            }
            finally
            {
                e.Handled = true;
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
