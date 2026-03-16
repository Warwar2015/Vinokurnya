using System;
using System.IO;
using System.Windows;
using VinokurnyaWpf.Data;

namespace VinokurnyaWpf.Services
{
    public class ThemeService
    {
        private const string ThemePreferenceFile = "theme_preference.txt";
        private static ThemeService? _instance;
        public static ThemeService Instance => _instance ??= new ThemeService();

        private ThemeType _currentTheme = ThemeType.Dark;
        private ThemeType _themePreference = ThemeType.Dark;

        public ThemeType CurrentTheme => _currentTheme;

        public void InitializeTheme()
        {
            try
            {
                LoadThemePreference();
                ApplyTheme(_themePreference);
            }
            catch (Exception)
            {
                ApplyTheme(ThemeType.Dark);
            }
        }

        public void LoadThemePreference()
        {
            try
            {
                if (File.Exists(ThemePreferenceFile))
                {
                    string preference = File.ReadAllText(ThemePreferenceFile).Trim();
                    if (Enum.TryParse<ThemeType>(preference, out ThemeType parsedTheme))
                    {
                        _themePreference = parsedTheme;
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
                File.WriteAllText(ThemePreferenceFile, _themePreference.ToString());
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
                if (theme == ThemeType.Light)
                {
                    app.Resources.MergedDictionaries[0].Source = new Uri("pack://application:,,,/Resources/Themes/Light.xaml", UriKind.Absolute);
                }
                else
                {
                    app.Resources.MergedDictionaries[0].Source = new Uri("pack://application:,,,/Resources/Themes/Dark.xaml", UriKind.Absolute);
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
    }

    public enum ThemeType
    {
        Dark,
        Light
    }
}