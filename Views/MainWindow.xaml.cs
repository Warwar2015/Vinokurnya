using System;
using System.Windows;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel? _viewModel;

        public MainWindow()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== MainWindow конструктор ===");
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("MainWindow InitializeComponent завершен");

                // Initialize ViewModel with error handling
                try
                {
                    _viewModel = new MainViewModel();
                    DataContext = _viewModel;
                    System.Diagnostics.Debug.WriteLine("MainViewModel создан и назначен");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании MainViewModel:\n{ex.Message}\n\n{ex.StackTrace}",
                        "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                // Handle closing event
                Closing += (sender, e) =>
                {
                    _viewModel?.Cleanup();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании главного окна:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Close the window on error
                this.Close();
            }
        }
    }
}
