using System;
using System.IO;
using System.Windows;
using VinokurnyaWpf.Data;

namespace VinokurnyaWpf.Services
{
    public class ThemeService
    {
        private const string ThemePreferenceFile = "theme_preference.txt";
        private ThemeType _currentTheme = ThemeType.Dark;
        private ThemeType _themePreference = ThemeType.Dark;
        private static ThemeService? _instance;
        private static readonly object _lock = new object();

        static ThemeService()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ThemeService static constructor called");
                lock (_lock)
                {
                    _instance = new ThemeService();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ThemeService static constructor: {ex.Message}\n{ex.StackTrace}");
                // Set default instance
                _instance = new ThemeService();
            }
        }

        private ThemeService()
        {
            _currentTheme = ThemeType.Dark;
            _themePreference = ThemeType.Dark;
            System.Diagnostics.Debug.WriteLine("ThemeService instance created");
        }

        public ThemeType CurrentTheme => _currentTheme;

        public void LoadThemePreference()
        {
            try
            {
                string? appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appFolder = Path.Combine(appDataPath, "Vinokurnya");

                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }

                string themeFile = Path.Combine(appFolder, ThemePreferenceFile);
                if (File.Exists(themeFile))
                {
                    string preference = File.ReadAllText(themeFile).Trim();
                    if (Enum.TryParse<ThemeType>(preference, out ThemeType parsedTheme))
                    {
                        _themePreference = parsedTheme;
                        _currentTheme = parsedTheme; // Update current theme
                        System.Diagnostics.Debug.WriteLine($"Theme preference loaded: {parsedTheme}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading theme preference: {ex.Message}");
                // Use default preference
            }
        }

        public void SaveThemePreference()
        {
            try
            {
                string? appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appFolder = Path.Combine(appDataPath, "Vinokurnya");

                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }

                string themeFile = Path.Combine(appFolder, ThemePreferenceFile);
                File.WriteAllText(themeFile, _themePreference.ToString());
                System.Diagnostics.Debug.WriteLine($"Theme preference saved: {_themePreference}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving theme preference: {ex.Message}");
                // Silent fail
            }
        }

        public void ApplyTheme(ThemeType theme)
        {
            try
            {
                _currentTheme = theme;
                var app = Application.Current;

                if (app == null)
                {
                    System.Diagnostics.Debug.WriteLine("Application.Current is null in ApplyTheme");
                    return;
                }

                // Get assembly name
                var assembly = typeof(ThemeService).Assembly;
                var assemblyName = assembly.GetName().Name;
                System.Diagnostics.Debug.WriteLine($"Assembly name: {assemblyName}");

                // Build pack URI for themes
                string lightThemeUri = $"pack://application:,,,/{assemblyName};component/Resources/Themes/Light.xaml";
                string darkThemeUri = $"pack://application:,,,/{assemblyName};component/Resources/Themes/Dark.xaml";

                System.Diagnostics.Debug.WriteLine($"Applying theme: {theme}");
                System.Diagnostics.Debug.WriteLine($"Light theme URI: {lightThemeUri}");
                System.Diagnostics.Debug.WriteLine($"Dark theme URI: {darkThemeUri}");

                Uri themeUri = theme == ThemeType.Light
                    ? new Uri(lightThemeUri, UriKind.Absolute)
                    : new Uri(darkThemeUri, UriKind.Absolute);

                // Clear existing merged dictionaries
                app.Resources.MergedDictionaries.Clear();

                // Load theme
                var themeDictionary = new ResourceDictionary();
                themeDictionary.Source = themeUri;
                app.Resources.MergedDictionaries.Add(themeDictionary);

                System.Diagnostics.Debug.WriteLine($"Theme successfully applied: {theme}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error applying theme: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void ToggleTheme()
        {
            ApplyTheme(_currentTheme == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
        }

        public void SetTheme(ThemeType theme)
        {
            _themePreference = theme;
            ApplyTheme(theme);
            SaveThemePreference();
        }

        public static ThemeService Instance => _instance ??= new ThemeService();
    }

    public enum ThemeType
    {
        Dark,
        Light
    }
}
