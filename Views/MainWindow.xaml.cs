using System;
using System.Windows;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // Initialize ViewModel with error handling
                MainViewModel viewModel = new MainViewModel();
                DataContext = viewModel;

                // Handle closing event
                Closing += (sender, e) =>
                {
                    viewModel?.Cleanup();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании главного окна:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Close the window on error
                this.Close();
            }
        }
    }
}