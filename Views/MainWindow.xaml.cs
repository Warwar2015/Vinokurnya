using System.Windows;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Set DataContext
            DataContext = new MainViewModel();
            
            // Handle closing event
            Closing += (sender, e) => 
            {
                var viewModel = DataContext as MainViewModel;
                viewModel?.Cleanup();
            };
        }
    }
}