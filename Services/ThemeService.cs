using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
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
            // Initialize with dark theme by default
            lock (_lock)
            {
                _instance = new ThemeService();
            }
        }

        private ThemeService()
        {
            _currentTheme = ThemeType.Dark;
            _themePreference = ThemeType.Dark;
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
                    }
                }
            }
            catch (Exception)
            {
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
            }
            catch (Exception)
            {
                // Silent fail
            }
        }

        public void ApplyTheme(ThemeType theme)
        {
            _currentTheme = theme;
            var app = Application.Current;

            if (app != null)
            {
                try
                {
                    Uri themeUri = theme == ThemeType.Light
                        ? new Uri("pack://application:,,,/Resources/Themes/Light.xaml", UriKind.Absolute)
                        : new Uri("pack://application:,,,/Resources/Themes/Dark.xaml", UriKind.Absolute);

                    // Clear existing merged dictionaries
                    app.Resources.MergedDictionaries.Clear();

                    // Load theme
                    var themeDictionary = new ResourceDictionary();
                    themeDictionary.Source = themeUri;
                    app.Resources.MergedDictionaries.Add(themeDictionary);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error applying theme: {ex.Message}");
                }
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
