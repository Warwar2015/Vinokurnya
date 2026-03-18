using System;
using System.Windows;
using System.Windows.Threading;
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

        protected override void OnStartup()
        {
            StartupEventArgs e = StartupEventArgs.Empty;

            try
            {
                System.Diagnostics.Debug.WriteLine("=== Приложение запускается ===");

                base.OnStartup(e);
                System.Diagnostics.Debug.WriteLine("App.OnStartup completed");

                // Initialize database context options
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                    optionsBuilder.UseSqlite("Data Source=vinokurnya.db");
                    _dbContextOptions = optionsBuilder.Options;
                    System.Diagnostics.Debug.WriteLine("DbContextOptions создан");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании DbContextOptions:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                // Initialize services
                try
                {
                    _dbContext = new AppDbContext(_dbContextOptions);
                    _dataService = new DataService(_dbContext);
                    System.Diagnostics.Debug.WriteLine("DataService создан");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании DataService:\n{ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                // Initialize database
                try
                {
                    _dbContext.Database.EnsureCreated();
                    System.Diagnostics.Debug.WriteLine("База данных создана");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании базы данных:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                // Ensure ThemeService is initialized
                try
                {
                    var themeService = ThemeService.Instance;
                    themeService.LoadThemePreference();
                    themeService.ApplyTheme(themeService.CurrentTheme);
                    System.Diagnostics.Debug.WriteLine("Тема загружена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при инициализации темы:\n{ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                // Load sample data asynchronously
                _ = Task.Run(async () =>
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Начало загрузки образцовых данных");
                        await _dataService.EnsureSampleDataAsync();
                        System.Diagnostics.Debug.WriteLine("Образцовые данные загружены");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке образцовых данных:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске приложения:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                // Save theme preference
                ThemeService.SaveThemePreference();
                System.Diagnostics.Debug.WriteLine("Приложение завершает работу");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при выходе: {ex.Message}");
            }
            finally
            {
                // Clean up
                _dbContext?.Dispose();
                base.OnExit(e);
            }
        }
    }
}
